using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using ubank_api.Data.Helpers.AuthHelpers;
using ubank_api.Data;
using ubank_api.Services.Interfaces;
using System.Security.Claims;
using ubank_api.Data.Models.Entities;
using ubank_api.Data.Models.Out;

namespace ubank_api.Services
{
    public enum TokenType
    {
        login,
        reset
    }

    public class AuthService : IAuthService
    {
        private readonly DatabaseContext _context;
        private readonly ISendGridEmailService _sendGridEmailService;
        private IConfiguration Configuration { get; set; }

        public AuthService(DatabaseContext context, IConfiguration configuration, ISendGridEmailService sendGridEmailService)
        {
            _context = context;

            Configuration = configuration;

            _sendGridEmailService = sendGridEmailService;
        }

        public TokenModel GetNewToken(string tokenModel)
        {
            var principal = GetPrincipalFromExpiredToken(tokenModel);
            if (principal == null)
            {
                throw new ArgumentException(String.Format("Invalid refresh token"));
            }
            var user = _context.Users.Any(user => user.Email == principal.FindFirstValue("Email"));

            if (!user || DateTimeOffset.FromUnixTimeSeconds(long.Parse(principal.FindFirst("exp")!.Value)).LocalDateTime < DateTime.Now)
            {
                throw new ArgumentException(String.Format("Invalid refresh token"));
            }

            return CreateToken(principal.Claims.ToList(), TokenType.login);
        }

        public bool ResetPassword(ResetPassword resetPassword)
        {
            string? accessToken = resetPassword.Token;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                throw new ArgumentException(String.Format("Invalid token"));
            }
            var user = _context.Users.Where(user => user.Email == principal.FindFirstValue("Email")).SingleOrDefault();

            if (DateTimeOffset.FromUnixTimeSeconds(long.Parse(principal.FindFirst("exp")!.Value)).LocalDateTime <= DateTime.Now)
            {
                throw new ArgumentException(String.Format("Expired token"));
            }

            if (user == null)
            {
                throw new ArgumentException(String.Format("User doesn't exist"));
            }

            CreatePasswordHash(resetPassword.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userUpdate = user;
            userUpdate.PasswordHash = passwordHash;
            userUpdate.PasswordSalt = passwordSalt;
            _context.Entry(user).CurrentValues.SetValues(userUpdate);
            _context.SaveChanges();
            return true;
        }

        public TokenModel UserLogin(UserLogin userIn)
        {
            var user = _context.Users.Where(user => user.Email == userIn.Email).SingleOrDefault();

            if (user != null && VerifyPasswordHash(user, userIn.Password))
            {
                List<Claim> claims = new()
                {
                    new Claim("Email", user.Email),
                    new Claim("Role", string.Concat(user.Role.ToString()[..1].ToLower(), (user.Role).ToString().AsSpan(1))),
                    new Claim("FirstName", user.FirstName),
                    new Claim(ClaimTypes.Role,  string.Concat(user.Role.ToString()[..1].ToLower(), (user.Role).ToString().AsSpan(1)))
                };

                return CreateToken(claims, TokenType.login);
            }
            else
            {
                throw new ArgumentException(String.Format("Email or password is incorrect."));
            }
        }


        public void SendActivationEmail(string emailIn)
        {
            var user = _context.Users.Where(user => user.Email == emailIn).SingleOrDefault();

            if (user != null)
            {
                SendEmail(user.Email);
            }
        }

        private void SendEmail(string email)
        {
            List<Claim> claims = new()
            {
                new Claim("Email", email)
            };
            var user = _context.Users.Where(user => user.Email == email).SingleOrDefault();
            var token = CreateToken(claims, TokenType.reset);
            var info = new EmailInfo
            {
                Email = email,
                Name = user!.FirstName,
                TemplateID = Configuration.GetSection("EmailSendGrid:TemplateID:PasswordReset").Value!,
                ResendRequest = Configuration.GetSection("EmailSendGrid:FrontendLink:ResendRequest").Value!,
                Url = Configuration.GetSection("EmailSendGrid:FrontendLink:PasswordReset").Value!,
                Token = token.AccessToken
            };
            var response = _sendGridEmailService.SendEmail(info);

        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(User user, string password)
        {
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(user.PasswordHash);
        }

        private TokenModel CreateToken(List<Claim> claims, TokenType type)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:Secret").Value!));
            int tokenValidityInMinutes;

            if (type == TokenType.login)
            {
                _ = int.TryParse(Configuration.GetSection("JWT:TokenValidityInMinutes").Value, out int temp);
                tokenValidityInMinutes = temp;
            }
            else
            {
                _ = int.TryParse(Configuration.GetSection("JWT:TokenValidityInMinutesReset").Value, out int temp);
                tokenValidityInMinutes = temp;
            }

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                signingCredentials: cred
                );

            var jwt = new TokenModel(new JwtSecurityTokenHandler().WriteToken(token), GenerateRefreshToken(claims));
            return jwt;
        }

        private string GenerateRefreshToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:Secret").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            _ = int.TryParse(Configuration.GetSection("JWT:RefreshTokenValidityInDays").Value, out int refreshTokenValidityInDays);
            var token = new JwtSecurityToken(
                  claims: claims,
                  expires: DateTime.UtcNow.AddDays(refreshTokenValidityInDays),
                  signingCredentials: cred
                  );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:Secret").Value!)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                ClaimsPrincipal? principal;
                principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
