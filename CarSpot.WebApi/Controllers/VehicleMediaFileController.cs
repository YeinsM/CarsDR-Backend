using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


[ApiController]
[Route("api/[controller]")]
public class VehicleMediaController : ControllerBase
{
    private readonly IVehicleMediaFileRepository _repository;
    private readonly IPhotoService _photoService;

    public VehicleMediaController(IVehicleMediaFileRepository repository, IPhotoService photoService)
    {
        _repository = repository;
        _photoService = photoService;
    }

    [HttpPost("upload")]
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
    public async Task<IActionResult> GetByVehicle(Guid vehicleId, [FromQuery] int page = 1, [FromQuery] int pageSize = 100)
    {
        const int maxPageSize = 100;

        if (page <= 0)
            return BadRequest("Page must be greater than zero.");

        if (pageSize <= 0)
            pageSize = 1;
        else if (pageSize > maxPageSize)
            pageSize = maxPageSize;

        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _repository.GetByVehicleIdPagedAsync(vehicleId, page, pageSize, baseUrl);

        return Ok(result);
    }


}
