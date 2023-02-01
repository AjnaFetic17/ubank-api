using System.Net.Mail;
using System.Net;
using ubank_api.Data.Helpers.AuthHelpers;
using ubank_api.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ubank_api.Services
{
    public class SendGridEmailService : ISendGridEmailService
    {
        private IConfiguration Configuration { get; set; }

        public SendGridEmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<HttpStatusCode> SendEmail(EmailInfo info)
        {
            return await Execute(info, Configuration);
        }

        static async Task<HttpStatusCode> Execute(EmailInfo info, IConfiguration Configuration)
        {
            var templateData = new Dictionary<string, string>
            {
                {"name", info.Name},
                {"email", info.Email},
                {"requestResend", info.ResendRequest!},
                {"token", info.Url + info.Token}
            };
            var apiKey = Configuration.GetSection("EmailSendGrid:API_KEY").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(Configuration.GetSection("EmailSendGrid:SenderEmail").Value, Configuration.GetSection("EmailSendGrid:EmailHeader").Value);
            var to = new EmailAddress(info.Email, info.Name);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, info.TemplateID, templateData);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            return response.StatusCode;
        }
    }
}
