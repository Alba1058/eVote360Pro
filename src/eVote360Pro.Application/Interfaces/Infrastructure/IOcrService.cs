namespace eVote360Pro.Core.Application.Interfaces.Infrastructure
{
    public interface IOcrService
    {
        Task<string?> ExtractDocumentNumberAsync(string imagePath);
    }
}
