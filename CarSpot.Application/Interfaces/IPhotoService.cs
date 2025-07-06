using Microsoft.AspNetCore.Http;
using CarSpot.Application.DTOs;
using System.Threading.Tasks;

public interface IPhotoService
{
    Task<PhotoUploadResult?> UploadImageAsync(IFormFile file);
    Task DeleteImageAsync(Guid ListingId);

}
