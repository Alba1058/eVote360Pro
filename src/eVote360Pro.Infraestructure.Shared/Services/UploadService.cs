using eVote360Pro.Core.Application.Interfaces.Infrastructure;
using System.IO;

namespace eVote360Pro.Infraestructure.Shared.Services
{
    public class UploadService : IUploadService
    {
        public async Task<string?> UploadFileAsync(Stream fileStream, string fileName, string folderName)
        {
            try
            {
                string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                
                if (!Directory.Exists(webRootPath))
                {
                    string baseDir = AppContext.BaseDirectory;
                    var directoryInfo = new DirectoryInfo(baseDir);
                    while (directoryInfo != null && !Directory.Exists(Path.Combine(directoryInfo.FullName, "wwwroot")))
                    {
                        directoryInfo = directoryInfo.Parent;
                    }
                    if (directoryInfo != null)
                    {
                        webRootPath = Path.Combine(directoryInfo.FullName, "wwwroot");
                    }
                    else
                    {
                        webRootPath = Path.Combine(baseDir, "wwwroot");
                    }
                }

                string targetFolder = Path.Combine(webRootPath, "images", folderName);
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                string filePath = Path.Combine(targetFolder, uniqueFileName);

                using (var localStream = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(localStream);
                }

                return $"/images/{folderName}/{uniqueFileName}";
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
