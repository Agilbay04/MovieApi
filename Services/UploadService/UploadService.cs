namespace MovieApi.Services.UploadService
{
    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment _env;

        public UploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No file uploaded");

            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName);

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Image format not allowed");

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "Uploads", folderName);
            if(!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine("Uploads", folderName, fileName);
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            var fullPath = Path.Combine(_env.ContentRootPath, filePath);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
            return Task.FromResult(true);
        }
    }
}