namespace MovieApi.Services.UploadService
{
    public interface IUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);

        Task<bool> DeleteFileAsync(string filePath);
    }
}