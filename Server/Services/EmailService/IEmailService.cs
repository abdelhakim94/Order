using System.Threading.Tasks;
using Order.Shared.Interfaces;

namespace Order.Server.Services.EmailService
{
    public interface IEmailService : IService
    {
        Task SendMail(string receiver, string subject, string body);
        Task SendEmailConfirmationMail(string receiver, string confirmationToken);
    }
}
