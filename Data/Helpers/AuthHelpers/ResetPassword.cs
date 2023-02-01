using System.ComponentModel.DataAnnotations;

namespace ubank_api.Data.Helpers.AuthHelpers
{
    public class ResetPassword
    {
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
        public string Password { get; set; } = string.Empty;
        public ResetPassword(string token, string password)
        {
            Token = token;
            Password = password;
        }
    }
}
