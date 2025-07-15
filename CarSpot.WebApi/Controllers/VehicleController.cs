using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Domain.Common;
using CarSpot.Application.Common.Helpers;




namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IMakeRepository _makeRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IAuxiliarRepository<Condition> _conditionRepository;
        private readonly IAuxiliarRepository<Color> _colorRepository;
        private readonly IAuxiliarRepository<VehicleType> _vehicleTypeRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVehicleMediaFileRepository _vehicleMediaRepository;
        private readonly IPhotoService _photoService;


        public VehicleController(
            IVehicleRepository vehicleRepository,
            IUserRepository userRepository,
            IMakeRepository makeRepository,
            IModelRepository modelRepository,
            IAuxiliarRepository<Condition> conditionRepository,
            IAuxiliarRepository<Color> colorRepository,
            IAuxiliarRepository<VehicleType> vehicleTypeRepository,
            IVehicleMediaFileRepository vehicleMediaFileRepository,
            IPhotoService photoService)
        {
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
            _makeRepository = makeRepository;
            _modelRepository = modelRepository;
            _conditionRepository = conditionRepository;
            _colorRepository = colorRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
            _vehicleMediaRepository = vehicleMediaFileRepository;
            _photoService = photoService;
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
            Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(id);
            return vehicle is null ? NotFound() : Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            User? user = await _userRepository.GetByIdAsync(request.UserId);
            Make make = await _makeRepository.GetByIdAsync(request.MakeId);
            Model? model = await _modelRepository.GetByIdAsync(request.ModelId);
            Condition? condition = await _conditionRepository.GetByIdAsync(request.ConditionId);
            Color? color = await _colorRepository.GetByIdAsync(request.ColorId);
            VehicleType? vehicleType = await _vehicleTypeRepository.GetByIdAsync(request.VehicleTypeId);


            if (user is null || make is null || model is null || condition is null || color is null || vehicleType is null)
            {
                return BadRequest("One or more required entities (User, Make, Model, Condition, Color, VehicleType) were not found.");
            }
            var vehicle = new Vehicle(
                request.VIN,
                request.UserId,
                request.MakeId,
                request.ModelId,
                request.VehicleTypeId,
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
                request.Title!,
                request.IsFeatured,
                request.FeaturedUntil,
                0,
                DateTime.UtcNow
            );

            vehicle.User = user;
            vehicle.Make = make;
            vehicle.Model = model;
            vehicle.Condition = condition;
            vehicle.Color = color;
            vehicle.VehicleType = vehicleType;

            vehicle.MediaFiles = [];
            vehicle.Comments = [];

            await _vehicleRepository.CreateVehicleAsync(vehicle);


            var response = new VehicleResponse(
                vehicle.Id,
                vehicle.Title!,
                vehicle.VIN,
                vehicle.VehicleType.Name,
                make.Name,
                model.Name!,
                condition.Name,
                color.Name,
                vehicle.Year,
                vehicle.Price
            );

            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, response);
        }


        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleRequest request)
        {
            Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle is null)
            {
                return NotFound();
            }
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
            Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle is null)
            {
                return NotFound();
            }

            await _vehicleRepository.DeleteAsync(vehicle);
            return NoContent();
        }

        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromBody] VehicleFilterRequest request)
        {
            var result = await _vehicleRepository.FilterAsync(request);
            return Ok(result);
        }





        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? orderBy = null,
            [FromQuery] string? sortDir = "asc")
        {
            orderBy = string.IsNullOrWhiteSpace(orderBy) ? "CreatedAt" : orderBy;

            IQueryable<Vehicle> query = _vehicleRepository.Query();

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            PaginatedResponse<Vehicle> result = await PaginationHelper.CreatePaginatedResponse(query, page, pageSize, baseUrl, orderBy, sortDir);

            return Ok(result);
        }



    }
}
