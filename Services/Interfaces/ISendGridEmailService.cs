using System.Net;
using ubank_api.Data.Helpers.AuthHelpers;

namespace ubank_api.Services.Interfaces
{
    public interface ISendGridEmailService
    {
        Task<HttpStatusCode> SendEmail(EmailInfo info);

    }
}
