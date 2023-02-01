using ubank_api.Data.Helpers.AuthHelpers;
using ubank_api.Data.Models.Entities;
using ubank_api.Data.Models.Out;

namespace ubank_api.Services.Interfaces
{
    public interface IAuthService
    {
        TokenModel UserLogin(UserLogin userIn);

        TokenModel GetNewToken(string tokenModel);

        bool ResetPassword(ResetPassword resetPassword);

        void SendActivationEmail(string emailIn);

        bool VerifyPasswordHash(User user, string password);

        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}
