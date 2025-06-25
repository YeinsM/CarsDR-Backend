using Microsoft.AspNetCore.Http;

public interface IPhotoService
{
    Task<string?> UploadImageAsync(IFormFile file);
}
