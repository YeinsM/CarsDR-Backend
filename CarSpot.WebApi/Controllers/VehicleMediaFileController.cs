using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class VehicleMediaController(IVehicleMediaFileRepository repository, IPhotoService photoService, IPaginationService paginationService) : ControllerBase
{
    [HttpPost("upload")]
    [Authorize(Policy = "AdminOrUser")]
    public async Task<IActionResult> Upload([FromForm] UploadVehicleMediaFileRequest request)
    {
        foreach (Microsoft.AspNetCore.Http.IFormFile file in request.Files)
        {
            string contentType = file.ContentType.ToLower();

            string mediaType = contentType.StartsWith("image") ? "image"
                              : contentType.StartsWith("video") ? "video"
                              : throw new InvalidOperationException("Unsupported media type");

            PhotoUploadResult? result = mediaType == "image"
                ? await photoService.UploadImageAsync(file)
                : await photoService.UploadVideoAsync(file);

            if (result == null)
            {
                continue;
            }

            var media = new VehicleMediaFile
            {
                Id = Guid.NewGuid(),
                VehicleId = request.VehicleId,
                Url = result.Url,
                PublicId = result.PublicId,
                MediaType = mediaType
            };

            await repository.AddAsync(media);
        }

        await repository.SaveChangesAsync();
        return Ok("Media uploaded successfully.");
    }


    
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id)
    {
        VehicleMediaFile? media = await repository.GetByIdAsync(id);
        if (media == null)
        {
            return NotFound();
        }

        if (media.MediaType == "image")
        {
            await photoService.DeleteImageAsync(media.PublicId);
        }
        else if (media.MediaType == "video")
        {
            await photoService.DeleteVideoAsync(media.PublicId);
        }

        await repository.DeleteAsync(id);
        await repository.SaveChangesAsync();

        return Ok("Media deleted successfully.");
    }

   
    [HttpGet("vehicle/{vehicleId}")]
     [AllowAnonymous]
    public async Task<ActionResult<PaginatedResponse<VehicleMediaFile>>> GetByVehicle(Guid vehicleId, [FromQuery] PaginationParameters pagination)
    {
        var (pageNumber, pageSize) = PaginationHelper.ValidateParameters(pagination);
        string baseUrl = PaginationHelper.BuildBaseUrl(Request);

        System.Linq.IQueryable<VehicleMediaFile> query = repository.Query()
            .Where(m => m.VehicleId == vehicleId);

        PaginatedResponse<VehicleMediaFile> paginatedResult = await paginationService.PaginateAsync(
            query,
            pageNumber,
            pageSize,
            baseUrl
        );

        return Ok(paginatedResult);
    }

}
