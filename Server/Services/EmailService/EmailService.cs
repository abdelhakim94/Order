using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Order.Server.Dto.Users;
using Order.Shared.Contracts;

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

        public Task SendEmailConfirmationMail(string receiver, string confirmationUrl)
        {
            var subject = "Confirmation de votre adresse mail";
            var body = new StringBuilder();
            body.AppendLine("<p>Félicitations! vous êtes à une étape de la finalisation de la création de votre compte.</p>");
            body.AppendLine($"<p>Veuillez cliquer <a href=\"{ confirmationUrl}\">ici</a> afin de confirmer votre adresse mail.</p>");
            body.AppendLine("<p>Ceci est un email automatique. Veuillez ne pas y répondre.</p>");
            body.AppendLine("<p>Cordialement,</p>");
            body.AppendLine("<p>l'équipe Order</p>");
            return SendMail(receiver, subject, body.ToString());
        }

        public Task ReSendEmailConfirmationMail(string receiver, string confirmationUrl)
        {
            var subject = "Confirmation de votre adresse mail";
            var body = new StringBuilder();
            body.AppendLine("<p>Il semble que vous ayez essayé de confirmer votre e-mail avec un lien expiré.</p>");
            body.AppendLine($"<p>Veuillez cliquer <a href=\"{confirmationUrl}\">ici</a> afin de confirmer votre adresse mail.</p>");
            body.AppendLine("<p>Ceci est un email automatique. Veuillez ne pas y répondre.</p>");
            body.AppendLine("<p>Cordialement,</p>");
            body.AppendLine("<p>L'équipe Order</p>");
            return SendMail(receiver, subject, body.ToString());
        }

        public Task SendResetPasswordMail(string receiver, string resetPwUrl)
        {
            var subject = "Réinitialisation de votre mot de passe";
            var body = new StringBuilder();
            body.AppendLine("<p>Vous avez demander à réinitialiser votre mot de passe <em>Order</em></p>");
            body.AppendLine($"<p>Veuillez cliquer <a href=\"{resetPwUrl}\">ici</a> afin de finaliser la réinitialisation.</p>");
            body.AppendLine("<p>Si vous n'êtes pas à l'origine de cette activité, veuillez ignorer ce mail.</p>");
            body.AppendLine("<p>Ceci est un email automatique. Veuillez ne pas y répondre.</p>");
            body.AppendLine("<p>Cordialement,</p>");
            body.AppendLine("<p>L'équipe Order</p>");
            return SendMail(receiver, subject, body.ToString());
        }

        public Task ReSendResetPasswordMail(string receiver, string resetPwUrl)
        {
            var subject = "Réinitialisation de votre mot de passe";
            var body = new StringBuilder();
            body.AppendLine("<p>Il semble que vous ayez essayé de réinitialiser votre mot de passe avec un lien expiré.</p>");
            body.AppendLine($"<p>Veuillez cliquer <a href=\"{resetPwUrl}\">ici</a> afin de finaliser la réinitialisation.</p>");
            body.AppendLine("<p>Si vous n'êtes pas à l'origine de cette activité, veuillez ignorer ce mail.</p>");
            body.AppendLine("<p>Ceci est un email automatique. Veuillez ne pas y répondre.</p>");
            body.AppendLine("<p>Cordialement,</p>");
            body.AppendLine("<p>L'équipe Order</p>");
            return SendMail(receiver, subject, body.ToString());
        }

        public Task SendExternalProviderEmailConfirmationEmail(string receiver, string confirmationUrl, string provider)
        {
            var subject = $"Confirmer la connexion avec le compte {provider}";
            var body = new StringBuilder();
            body.Append($"<p>Veuillez confirmer l'association de votre compte {provider} en cliquant <a href=\"{ confirmationUrl}\">ici</a>.</p>");
            body.AppendLine("<p>Ceci est un email automatique. Veuillez ne pas y répondre.</p>");
            body.AppendLine("<p>Cordialement,</p>");
            body.AppendLine("<p>L'équipe Order</p>");
            return SendMail(receiver, subject, body.ToString());
        }
    }
}
