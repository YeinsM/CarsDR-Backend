using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IModelRepository _modelRepository;

        public VehicleController(IVehicleRepository vehicleRepository, IModelRepository modelRepository)
        {
            _vehicleRepository = vehicleRepository;
            _modelRepository = modelRepository;
        }

        

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _vehicleRepository.GetAllAsync());
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
            /*var model = await _modelRepository.GetByIdAsync(request.ModelId);
            if (model is null)
            {
                return BadRequest("Model not found.");
            }*/

            var vehicle = new Vehicle(request.VIN, request.Year, request.ModelId, request.Color);
            await _vehicleRepository.AddAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }
    }
}
