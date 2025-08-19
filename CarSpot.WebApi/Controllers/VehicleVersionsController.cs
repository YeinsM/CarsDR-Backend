using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces.Services;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleVersionsController : ControllerBase
    {
        private readonly IAuxiliarRepository<VehicleVersion> _repository;
        private readonly IModelRepository _modelRepository;
        private readonly IPaginationService _paginationService;

        public VehicleVersionsController(
            IAuxiliarRepository<VehicleVersion> repository,
            IModelRepository modelRepository,
            IPaginationService paginationService
        )
        {
            _repository = repository;
            _modelRepository = modelRepository;
            _paginationService = paginationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<VehicleVersionDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            if (pageNumber <= 0)
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "Page number must be greater than zero."));

            var query = _repository.Query();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query.Select(vv => new VehicleVersionDto(
                    vv.Id,
                    vv.Name!,
                    vv.ModelId
                )),
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }

 
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(int id)
        {
            var version = await _repository.GetByIdAsync(id);
            if (version is null)
                return NotFound(ApiResponseBuilder.Fail<VehicleVersion>(404, $"Vehicle version with ID {id} not found."));

            return Ok(ApiResponseBuilder.Success(version));
        }

        
        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create([FromBody] VehicleVersion vehicleVersion)
        {
            var model = await _modelRepository.GetByIdAsync(vehicleVersion.ModelId);
            if (model is null)
                return BadRequest(ApiResponseBuilder.Fail<VehicleVersion>(400, $"Model with ID {vehicleVersion.ModelId} does not exist."));

            await _repository.Add(vehicleVersion);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = vehicleVersion.Id },
                ApiResponseBuilder.Success(vehicleVersion, "Vehicle version created successfully."));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleVersion updated)
        {
            if (id != updated.Id)
                return BadRequest(ApiResponseBuilder.Fail<VehicleVersion>(400, "The ID in the URL does not match the ID in the payload."));

            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
                return NotFound(ApiResponseBuilder.Fail<VehicleVersion>(404, $"Vehicle version with ID {id} not found."));

            var model = await _modelRepository.GetByIdAsync(updated.ModelId);
            if (model is null)
                return BadRequest(ApiResponseBuilder.Fail<VehicleVersion>(400, $"Model with ID {updated.ModelId} does not exist."));

            existing.Name = updated.Name;
            existing.ModelId = updated.ModelId;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(existing, "Vehicle version updated successfully."));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var version = await _repository.GetByIdAsync(id);
            if (version is null)
                return NotFound(ApiResponseBuilder.Fail<VehicleVersion>(404, $"Vehicle version with ID {id} not found."));

            await _repository.DeleteAsync(version);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<VehicleVersion>(null, "Vehicle version deleted successfully."));
        }
    }
}
