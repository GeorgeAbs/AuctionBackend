using Domain.Constants;
using MimeKit;
using Domain.Interfaces.Services;


namespace Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender() { }

        public async Task<string> SendEmailAsync(string emailTo, string subject, string htmlBody)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("ппп", MailSettingsConstants.EMAIL_SENDER));
            message.To.Add(new MailboxAddress("ggg", emailTo));
            message.Subject = subject;
            message.Body = new BodyBuilder() { HtmlBody = htmlBody }.ToMessageBody();

            using MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(MailSettingsConstants.SMTP_CLIENT, MailSettingsConstants.SMTP_PORT, true);
            await client.AuthenticateAsync(MailSettingsConstants.EMAIL_SENDER, MailSettingsConstants.EMAIL_SENDER_PWD);
            var responce = await client.SendAsync(message);

            await client.DisconnectAsync(true);

            return responce;
        }
    }
}
