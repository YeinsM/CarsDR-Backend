using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IAuxiliarRepository<Make> _makeRepository;
        private readonly IAuxiliarRepository<Model> _modelRepository;
        private readonly IAuxiliarRepository<Condition> _conditionRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserRepository _userRepository;

        public VehicleController(
            IVehicleRepository vehicleRepository,
            IUserRepository userRepository,
            IAuxiliarRepository<Make> makeRepository,
            IAuxiliarRepository<Model> modelRepository,
            IAuxiliarRepository<Condition> conditionRepository)
        {
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
            _makeRepository = makeRepository;
            _modelRepository = modelRepository;
            _conditionRepository = conditionRepository;
        }

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
            var user = await _userRepository.GetByIdAsync(request.UserId);
            var make = await _makeRepository.GetByIdAsync(request.MakeId);
            var model = await _modelRepository.GetByIdAsync(request.ModelId);
            var condition = await _conditionRepository.GetByIdAsync(request.ConditionId);

            if (user is null || make is null || model is null || condition is null)
                return BadRequest("One or more required entities (User, Make, Model, Condition) were not found.");

            var vehicle = new Vehicle(
                request.VIN,
                request.UserId,
                request.MakeId,
                request.ModelId,
                request.VehicleVersionId,
                request.MarketVersionId,
                request.TransmissionId,
                request.DrivetrainId,
                request.CylinderOptionId,
                request.CabTypeId,
                request.ConditionId,
                request.ColorId,
                request.Year,
                request.Mileage,
                request.Price,
                request.Title,
                request.IsFeatured,
                request.FeaturedUntil,
                0,
                DateTime.UtcNow
            );

            
            vehicle.User = user;
            vehicle.Make = make;
            vehicle.Model = model;
            vehicle.Condition = condition;

            
            vehicle.Images = [];
            vehicle.Comments = [];

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
            vehicle.VehicleVersionId = request.VehicleVersionId;
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
