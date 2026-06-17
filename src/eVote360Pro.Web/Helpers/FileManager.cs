namespace eVote360Pro.Web.Helpers
{
    public static class FileManager
    {
        public static async Task<string?> UploadAsync(IFormFile? file, string folderName, string? currentPath = null)
        {
            if (file == null || file.Length == 0)
                return currentPath;

            var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var targetFolder = Path.Combine(webRoot, "images", folderName);
            Directory.CreateDirectory(targetFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(targetFolder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/{folderName}/{fileName}";
        }
    }
}
