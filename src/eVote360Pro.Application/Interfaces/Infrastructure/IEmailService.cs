namespace eVote360Pro.Core.Application.Interfaces.Infrastructure
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
