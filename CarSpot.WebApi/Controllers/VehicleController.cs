using System;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CarSpot.Application.Common.Responses;




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
            VehicleType? vehicleType = await _vehicleTypeRepository.GetByIdAsync(request.VehicleTypeId);

            if (user is null || make is null || model is null || condition is null || vehicleType is null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ApiResponseBuilder.Fail<object?>(
                        StatusCodes.Status400BadRequest,
                        "One or more required entities (User, Make, Model, Condition, VehicleType) were not found."));
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
                request.Title,
                request.IsFeatured,
                request.FeaturedUntil,
                0,
                DateTime.UtcNow
            );

            vehicle.User = user;
            vehicle.Make = make;
            vehicle.Model = model;
            vehicle.VehicleType = vehicleType;
            vehicle.MediaFiles = [];

            await _vehicleRepository.CreateVehicleAsync(vehicle);

            var response = new VehicleResponse(
                vehicle.Id,
                vehicle.Title!,
                vehicle.VIN,
                vehicle.VehicleType.Name,
                make.Name,
                model.Name!,
                condition.Name,
                vehicle.Year,
                vehicle.Price
            );

            return StatusCode(StatusCodes.Status201Created,
                ApiResponseBuilder.Success(response, "Vehicle created successfully."));
        }



        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleRequest request)
        {
            Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle is null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    ApiResponseBuilder.Fail<object?>(
                        StatusCodes.Status404NotFound,
                        $"Vehicle with ID {id} does not exist."));
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

            return StatusCode(StatusCodes.Status204NoContent,
                ApiResponseBuilder.Success<object?>(null, "Vehicle updated successfully."));
        }


        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle is null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    ApiResponseBuilder.Fail<object?>(
                        StatusCodes.Status404NotFound,
                        $"Vehicle with ID {id} does not exist."));
            }

            await _vehicleRepository.DeleteAsync(vehicle);

            return StatusCode(StatusCodes.Status204NoContent,
                ApiResponseBuilder.Success<object?>(null, "Vehicle deleted successfully."));
        }


        [HttpPost("filter")]
        public async Task<IActionResult> FilterVehicles([FromBody] VehicleFilterRequest request)
        {
            if (request.MinMileage.HasValue && request.MaxMileage.HasValue &&
                request.MinMileage > request.MaxMileage)
            {
                return BadRequest("MinMileage cannot be greater than MaxMileage.");
            }

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return BadRequest("Page and PageSize must be greater than 0.");
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var result = await _vehicleRepository.FilterAsync(request, baseUrl);

            return Ok(result);
        }


    }
}
