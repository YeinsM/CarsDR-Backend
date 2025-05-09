using CarSpot.Application.DTOs;
using CarSpot.Application.DTOs.Vehicle;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController(IVehicleRepository _vehicleRepository) : ControllerBase
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
            return vehicle is null ? NotFound() : Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                VIN = request.VIN,
                UserId = request.UserId,
                MakeId = request.MakeId,
                ModelId = request.ModelId,
                VersionId = request.VersionId,
                MarketVersionId = request.MarketVersionId,
                TransmissionId = request.TransmissionId,
                DrivetrainId = request.DrivetrainId,
                CylinderOptionId = request.CylinderOptionId,
                CabTypeId = request.CabTypeId,
                ConditionId = request.ConditionId,
                ColorId = request.ColorId,
                Year = request.Year,
                Mileage = request.Mileage,
                Price = request.Price,
                Title = request.Title,
                IsFeatured = request.IsFeatured,
                FeaturedUntil = request.FeaturedUntil,
                CreatedAt = DateTime.UtcNow,
                ViewCount = 0,
                Images = [],
                Comments = []
            };

            await _vehicleRepository.AddAsync(vehicle);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleRequest request)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle is null)
                return NotFound();

            vehicle.MakeId = request.MakeId;
            vehicle.ModelId = request.ModelId;
            vehicle.VersionId = request.VersionId;
            vehicle.MarketVersionId = request.MarketVersionId;
            vehicle.TransmissionId = request.TransmissionId;
            vehicle.DrivetrainId = request.DrivetrainId;
            vehicle.CylinderOptionId = request.CylinderOptionId;
            vehicle.CabTypeId = request.CabTypeId;
            vehicle.ConditionId = request.ConditionId;
            vehicle.ColorId = request.ColorId;
            vehicle.Year = request.Year;
            vehicle.Mileage = request.Mileage;
            vehicle.Price = request.Price;
            vehicle.Title = request.Title;
            vehicle.IsFeatured = request.IsFeatured;
            vehicle.FeaturedUntil = request.FeaturedUntil;

            await _vehicleRepository.UpdateAsync(vehicle);
            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle is null)
                return NotFound();

            await _vehicleRepository.DeleteAsync(vehicle);
            return NoContent();
        }
    }
}
