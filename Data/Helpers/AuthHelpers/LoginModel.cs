namespace ubank_api.Data.Helpers.AuthHelpers
{
    public class LoginModel
    {
        public UserLogin? User { get; set; }

        public string GrantType { get; set; } = string.Empty;

        public string? RefreshToken { get; set; } = string.Empty;
    }
}
