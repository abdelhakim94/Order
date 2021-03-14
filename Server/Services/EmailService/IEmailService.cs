using System.Threading.Tasks;
using Order.Shared.Contracts;

namespace Order.Server.Services.EmailService
{
    public interface IEmailService : IService
    {
        Task SendMail(string receiver, string subject, string body);

        Task SendEmailConfirmationMail(string receiver, string confirmationUrl);
        Task ReSendEmailConfirmationMail(string receiver, string confirmationUrl);

        Task SendResetPasswordMail(string receiver, string resetPwUrl);
        Task ReSendResetPasswordMail(string receiver, string resetPwUrl);

        Task SendExternalProviderEmailConfirmationEmail(string receiver, string confirmationUrl, string provider);
    }
}
