using CarSpot.Infrastructure.Persistence.Context;
using CarSpot.Infrastructure.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarSpot.Infrastructure.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    private readonly ApplicationDbContext _context;

    public PhotoService(IOptions<CloudinarySettings> config, ApplicationDbContext context)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(acc);
        _context = context;
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

    public async Task DeleteImageAsync(Guid id)
    {

        var image = await _context.VehicleImages.FirstOrDefaultAsync(img => img.Id == id);
        if (image == null || string.IsNullOrWhiteSpace(image.PublicId))
            return;


        var deleteParams = new DeletionParams(image.PublicId);


        var result = await _cloudinary.DestroyAsync(deleteParams);


        if (result.Result != "ok")
        {
            throw new Exception($"Failed to delete image from Cloudinary. Status: {result.Result}");
        }
    }

}
