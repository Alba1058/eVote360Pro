namespace eVote360Pro.Core.Application.Interfaces.Infrastructure
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendVerificationCodeAsync(string to, string nombre, string codigo);
        Task SendVotingSummaryAsync(string to, string nombre, string eleccionNombre, DateTime fecha, IEnumerable<(string Puesto, string Seleccion, string Partido)> resumen);
    }
}
