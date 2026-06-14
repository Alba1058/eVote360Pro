using System.IO;

namespace eVote360Pro.Core.Application.Interfaces.Infrastructure
{
    public interface IUploadService
    {
        Task<string?> UploadFileAsync(Stream fileStream, string fileName, string folderName);
    }
}
