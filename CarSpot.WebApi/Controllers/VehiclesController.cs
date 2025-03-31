using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehiclesController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _vehicleRepository.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            return vehicle == null ? NotFound() : Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            var vehicle = new Vehicle(request.VIN, request.ModelId, request.Year, request.Color);
            await _vehicleRepository.AddAsync(vehicle);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }
    }
}
