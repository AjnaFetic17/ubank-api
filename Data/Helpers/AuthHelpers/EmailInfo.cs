namespace ubank_api.Data.Helpers.AuthHelpers
{
    public class EmailInfo
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string TemplateID { get; set; } = string.Empty;

        public string? ResendRequest { get; set; } = string.Empty;

        public string? ContactUs { get; set; } = string.Empty;
    }
}
