using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarSpot.Application.Common;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController(
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
        ) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<Vehicle>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            var (pageNumber, pageSize) = PaginationHelper.ValidateParameters(pagination);
            string baseUrl = PaginationHelper.BuildBaseUrl(Request);

            System.Linq.IQueryable<Vehicle> query = vehicleRepository.Query();

            PaginatedResponse<Vehicle> paginatedResult = await paginationService.PaginateAsync(
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
            Vehicle? vehicle = await vehicleRepository.GetByIdAsync(id);

            if (vehicle is null)
            {
                return NotFound(ApiResponseBuilder.Fail<object>(404, $"Vehicle with ID {id} not found."));
            }

            
            vehicle.ViewCount++;

            
            await vehicleRepository.UpdateAsync(vehicle);

            return Ok(ApiResponseBuilder.Success(vehicle, "Vehicle retrieved successfully."));
        }


        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            User? user = await userRepository.GetByIdAsync(request.UserId);
            Make? make = await makeRepository.GetByIdAsync(request.MakeId);
            Model? model = await modelRepository.GetByIdAsync(request.ModelId);
            Condition? condition = await conditionRepository.GetByIdAsync(request.ConditionId);
            VehicleType? vehicleType = await vehicleTypeRepository.GetByIdAsync(request.VehicleTypeId);

            if (user is null || make is null || model is null || condition is null || vehicleType is null)
            {
                return BadRequest(ApiResponseBuilder.Fail<object>(
                    400,
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
            vehicle.MediaFiles = new List<VehicleMediaFile>();

            await vehicleRepository.CreateVehicleAsync(vehicle);

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
            Vehicle? vehicle = await vehicleRepository.GetByIdAsync(id);
            if (vehicle is null)
            {
                return NotFound(ApiResponseBuilder.Fail<object>(404, $"Vehicle with ID {id} does not exist."));
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

            await vehicleRepository.UpdateAsync(vehicle);

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Vehicle? vehicle = await vehicleRepository.GetByIdAsync(id);
            if (vehicle is null)
            {
                return NotFound(ApiResponseBuilder.Fail<object>(404, $"Vehicle with ID {id} does not exist."));
            }

            await vehicleRepository.DeleteAsync(vehicle);

            return NoContent();
        }

        [HttpPost("filter")]
        [AllowAnonymous]
        public async Task<IActionResult> FilterVehicles([FromBody] VehicleFilterRequest request)
        {
            if (request.MinMileage.HasValue && request.MaxMileage.HasValue && request.MinMileage > request.MaxMileage)
            {
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "MinMileage cannot be greater than MaxMileage."));
            }

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "Page and PageSize must be greater than 0."));
            }

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            PaginatedResponse<VehicleDto> result = await vehicleRepository.FilterAsync(request, baseUrl);

            return Ok(ApiResponseBuilder.Success(result, "Filtered vehicles retrieved successfully."));
        }
    }
}
