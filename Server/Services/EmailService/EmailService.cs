using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Order.Server.Dto.Users;
using Order.Shared.Interfaces;

namespace Order.Server.Services.EmailService
{
    public class EmailService : IEmailService, IService
    {
        private readonly EmailBox sender;

        public EmailService(EmailBox sender)
        {
            this.sender = sender;
        }

        public Task SendMail(string receiver, string subject, string body)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(sender.Address, sender.DisplayName);
            mailMessage.To.Add(new MailAddress(receiver));

            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(sender.Address, sender.Password);
            client.Host = sender.SmtpHost;
            client.Port = sender.SmtpPort;

            client.EnableSsl = true;

            return client.SendMailAsync(mailMessage);
        }

        public Task SendEmailConfirmationMail(string receiver, string confirmationLink)
        {
            var subject = "Confirmation de l'adresse mail";
            var body = new StringBuilder();
            body.AppendLine("<p>Félicitations! vous êtes à une étape de la finalisation de la création de votre compte.</p>");
            body.AppendLine("<p>Veuillez suivre le lien ci-dessous pour confirmer votre adresse mail.</p>");
            body.AppendLine($"<p>{confirmationLink}</p>");
            body.AppendLine("<p>Ceci est un email automatique. Veuillez ne pas y répondre.</p>");
            body.AppendLine("<p>Cordialement,</p>");
            body.AppendLine("<p>l'équipe Order</p>");
            return SendMail(receiver, subject, body.ToString());
        }
    }
}
