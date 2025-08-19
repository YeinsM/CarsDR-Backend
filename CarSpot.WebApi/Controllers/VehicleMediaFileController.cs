using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CarSpot.Domain.Common;
using CarSpot.Application.Interfaces.Services;

[ApiController]
[Route("api/[controller]")]
public class VehicleMediaController : ControllerBase
{
    private readonly IVehicleMediaFileRepository _repository;
    private readonly IPhotoService _photoService;
    private readonly IPaginationService _paginationService;

    public VehicleMediaController(IVehicleMediaFileRepository repository, IPhotoService photoService, IPaginationService paginationService)
    {
        _repository = repository;
        _photoService = photoService;
        _paginationService = paginationService;
    }


    
    [HttpPost("upload")]
    [Authorize(Policy = "AdminOrUser")]
    public async Task<IActionResult> Upload([FromForm] UploadVehicleMediaFileRequest request)
    {
        foreach (var file in request.Files)
        {
            var contentType = file.ContentType.ToLower();

            string mediaType = contentType.StartsWith("image") ? "image"
                              : contentType.StartsWith("video") ? "video"
                              : throw new InvalidOperationException("Unsupported media type");

            var result = mediaType == "image"
                ? await _photoService.UploadImageAsync(file)
                : await _photoService.UploadVideoAsync(file);

            if (result == null)
                continue;

            var media = new VehicleMediaFile
            {
                Id = Guid.NewGuid(),
                VehicleId = request.VehicleId,
                Url = result.Url,
                PublicId = result.PublicId,
                MediaType = mediaType
            };

            await _repository.AddAsync(media);
        }

        await _repository.SaveChangesAsync();
        return Ok("Media uploaded successfully.");
    }


    
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var media = await _repository.GetByIdAsync(id);
        if (media == null)
            return NotFound();

        if (media.MediaType == "image")
            await _photoService.DeleteImageAsync(media.PublicId);
        else if (media.MediaType == "video")
            await _photoService.DeleteVideoAsync(media.PublicId);

        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();

        return Ok("Media deleted successfully.");
    }

   
    [HttpGet("vehicle/{vehicleId}")]
     [AllowAnonymous]
    public async Task<ActionResult<PaginatedResponse<Comment>>> GetByVehicle(Guid vehicleId, [FromQuery] PaginationParameters pagination)
    {
        const int maxPageSize = 100;

        int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
        int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

        var query = _repository.Query();

        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var paginatedResult = await _paginationService.PaginateAsync(
            query,
            pageNumber,
            pageSize,
            baseUrl
        );

        return Ok(paginatedResult);
    }

}
