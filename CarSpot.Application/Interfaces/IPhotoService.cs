using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using Microsoft.AspNetCore.Http;

public interface IPhotoService
{
    Task<PhotoUploadResult?> UploadImageAsync(IFormFile file);
    Task DeleteImageAsync(Guid ListingId);

}
