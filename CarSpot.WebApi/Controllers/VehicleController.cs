using System;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CarSpot.Application.Common.Responses;
using System.Collections.Generic;
using CarSpot.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using CarSpot.Application.Interfaces.Services;

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
        private readonly IPaginationService _paginationService;

        public VehicleController(
            IVehicleRepository vehicleRepository,
            IUserRepository userRepository,
            IMakeRepository makeRepository,
            IModelRepository modelRepository,
            IAuxiliarRepository<Condition> conditionRepository,
            IAuxiliarRepository<Color> colorRepository,
            IAuxiliarRepository<VehicleType> vehicleTypeRepository,
            IVehicleMediaFileRepository vehicleMediaFileRepository,
            IPhotoService photoService,
            IPaginationService paginationService
        )
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
            _paginationService = paginationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<Vehicle>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            var query = _vehicleRepository.Query();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query,
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }

        [HttpGet("{id:Guid}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);

            if (vehicle is null)
            {
                return NotFound(ApiResponseBuilder.Fail<object>(404, $"Vehicle with ID {id} not found."));
            }

            
            vehicle.ViewCount++;

            
            await _vehicleRepository.UpdateAsync(vehicle);

            return Ok(ApiResponseBuilder.Success(vehicle, "Vehicle retrieved successfully."));
        }


        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            User? user = await _userRepository.GetByIdAsync(request.UserId);
            Make? make = await _makeRepository.GetByIdAsync(request.MakeId);
            Model? model = await _modelRepository.GetByIdAsync(request.ModelId);
            Condition? condition = await _conditionRepository.GetByIdAsync(request.ConditionId);
            VehicleType? vehicleType = await _vehicleTypeRepository.GetByIdAsync(request.VehicleTypeId);

            if (user is null || make is null || model is null || condition is null || vehicleType is null)
                return BadRequest(ApiResponseBuilder.Fail<object>(
                    400,
                    "One or more required entities (User, Make, Model, Condition, VehicleType) were not found."));

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
            vehicle.MediaFiles = new List<VehicleMediaFile>();

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
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleRequest request)
        {
            Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle is null)
                return NotFound(ApiResponseBuilder.Fail<object>(404, $"Vehicle with ID {id} does not exist."));

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
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle is null)
                return NotFound(ApiResponseBuilder.Fail<object>(404, $"Vehicle with ID {id} does not exist."));

            await _vehicleRepository.DeleteAsync(vehicle);

            return NoContent();
        }

        [HttpPost("filter")]
        [AllowAnonymous]
        public async Task<IActionResult> FilterVehicles([FromBody] VehicleFilterRequest request)
        {
            if (request.MinMileage.HasValue && request.MaxMileage.HasValue && request.MinMileage > request.MaxMileage)
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "MinMileage cannot be greater than MaxMileage."));

            if (request.Page <= 0 || request.PageSize <= 0)
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "Page and PageSize must be greater than 0."));

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var result = await _vehicleRepository.FilterAsync(request, baseUrl);

            return Ok(ApiResponseBuilder.Success(result, "Filtered vehicles retrieved successfully."));
        }
    }
}
