using System;
using System.Threading.Tasks;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CarSpot.Application.DTOs;
using System.Linq;



namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleImagesController : ControllerBase
    {
        private readonly IVehicleImageRepository _repository;
        private readonly IPhotoService _photoService;

        public VehicleImagesController(
            IVehicleImageRepository repository,
            IPhotoService photoService)
        {
            _repository = repository;
            _photoService = photoService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleImageResponse>>> GetAll()
        {
            var images = await _repository.GetAllAsync();
            var response = images.Select(img => new VehicleImageResponse(img.Id, img.VehicleId, img.ImageUrl));
            return Ok(response);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<VehicleImageResponse>> GetById(Guid id)
        {
            var image = await _repository.GetByIdAsync(id);
            if (image is null)
                return NotFound(new { message = "Image not found." });

            var response = new VehicleImageResponse(image.Id, image.VehicleId, image.ImageUrl);
            return Ok(response);
        }


        [HttpGet("vehicle/{vehicleId:guid}")]
        public async Task<ActionResult<IEnumerable<VehicleImageResponse>>> GetByVehicleIdAsync(Guid vehicleId)
        {
            var images = await _repository.GetByVehicleIdAsync(vehicleId);
            var response = images.Select(img => new VehicleImageResponse(img.Id, img.VehicleId, img.ImageUrl));
            return Ok(response);
        }


        [Consumes("multipart/form-data")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateVehicleImageRequest request)
        {
            if (request.File is null || request.File.Length == 0)
                return BadRequest(new { message = "Image file is required." });

            var uploadResult = await _photoService.UploadImageAsync(request.File);
            if (uploadResult == null || string.IsNullOrWhiteSpace(uploadResult.Url))
                return StatusCode(500, new { message = "Image upload failed." });

            var image = new VehicleImage
            {
                Id = Guid.NewGuid(),
                VehicleId = request.VehicleId,
                ImageUrl = uploadResult.Url,
                PublicId = uploadResult.PublicId
            };

            await _repository.CreateAsync(image);

            var response = new VehicleImageResponse(image.Id, image.VehicleId, image.ImageUrl);
            return CreatedAtAction(nameof(GetById), new { id = image.Id }, response);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var image = await _repository.GetByIdAsync(id);
            if (image is null)
                return NotFound(new { message = "Image not found." });

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
