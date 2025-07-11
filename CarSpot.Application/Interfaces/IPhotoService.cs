using Microsoft.AspNetCore.Http;

public interface IPhotoService
{
    Task<PhotoUploadResult?> UploadImageAsync(IFormFile file);
Task<PhotoUploadResult?> UploadVideoAsync(IFormFile file);
Task DeleteImageAsync(string publicId);
Task DeleteVideoAsync(string publicId);


}
