namespace Domain.Interfaces.Services
{
    public interface IEmailSender
    {
        Task<string> SendEmailAsync(string emailTo, string subject, string htmlBody);
    }
}
