using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleVersionsController : ControllerBase
    {
        private readonly IAuxiliarRepository<VehicleVersion> _repository;
        private readonly IModelRepository _modelRepository;

        public VehicleVersionsController(
            IAuxiliarRepository<VehicleVersion> repository,
            IModelRepository modelRepository)
        {
            _repository = repository;
            _modelRepository = modelRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "Page and pageSize must be greater than zero."));

            var query = _repository.Query();

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResponse = new PaginatedResponse<VehicleVersion>(
                data: items,
                page: page,
                pageSize: pageSize,
                total: totalItems,
                baseUrl: baseUrl
            );

            return Ok(ApiResponseBuilder.Success(paginatedResponse, "Vehicle versions retrieved successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var version = await _repository.GetByIdAsync(id);
            if (version is null)
                return NotFound(ApiResponseBuilder.Fail<VehicleVersion>(404, $"Vehicle version with ID {id} not found."));
            return Ok(ApiResponseBuilder.Success(version));
        }

        [HttpPost]
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
