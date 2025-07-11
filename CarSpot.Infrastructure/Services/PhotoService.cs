
using CarSpot.Infrastructure.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(acc);
    }

    public async Task<PhotoUploadResult?> UploadImageAsync(IFormFile file)
    {
        if (file.Length <= 0) return null;

        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "vehicle-images"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return new PhotoUploadResult
        {
            Url = uploadResult.SecureUrl.ToString(),
            PublicId = uploadResult.PublicId
        };
    }

    public async Task<PhotoUploadResult?> UploadVideoAsync(IFormFile file)
    {
        if (file.Length <= 0) return null;

        await using var stream = file.OpenReadStream();
        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "vehicle-videos"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return new PhotoUploadResult
        {
            Url = uploadResult.SecureUrl.ToString(),
            PublicId = uploadResult.PublicId
        };
    }

    public async Task DeleteImageAsync(string publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId)) return;

        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Result != "ok")
        {
            throw new Exception($"Failed to delete image from Cloudinary. Status: {result.Result}");
        }
    }

    public async Task DeleteVideoAsync(string publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId)) return;

        var deleteParams = new DeletionParams(publicId)
        {
            ResourceType = ResourceType.Video
        };

        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Result != "ok")
        {
            throw new Exception($"Failed to delete video from Cloudinary. Status: {result.Result}");
        }
    }
}
