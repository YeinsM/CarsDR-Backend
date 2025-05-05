using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController(IRepository<Vehicle> _vehicleRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            return Ok(vehicles);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            return vehicle == null ? NotFound() : Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            var vehicle = new Vehicle(request.VIN, request.Year, request.ModelId, request.Color);
            await _vehicleRepository.AddAsync(vehicle);

            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleRequest request)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            vehicle = new Vehicle(request.VIN, request.Year, request.ModelId, request.Color); // Update logic
            await _vehicleRepository.UpdateAsync(vehicle);

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            await _vehicleRepository.DeleteAsync(vehicle);
            return NoContent();
        }
    }
}
