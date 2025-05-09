using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleImagesController : ControllerBase
    {
        private readonly IAuxiliarRepository<VehicleImage> _repository;

        public VehicleImagesController(IAuxiliarRepository<VehicleImage> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var images = await _repository.GetAllAsync();
            return Ok(images);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var image = await _repository.GetByIdAsync(id);
            return image == null ? NotFound() : Ok(image);
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicleId(Guid vehicleId)
        {
            var images = await _repository.GetByIdAsync(vehicleId);
            return Ok(images);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleImage image)
        {
            image.Id = Guid.NewGuid();
            await _repository.Add(image);
            return CreatedAtAction(nameof(GetById), new { id = image.Id }, image);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
