using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using CarSpot.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleVersionsController(
        IAuxiliarRepository<VehicleVersion> repository,
        IModelRepository modelRepository,
        IPaginationService paginationService
        ) : PaginatedControllerBase(paginationService)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<VehicleVersionDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            if (pagination.PageNumber <= 0)
            {
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "Page number must be greater than zero."));
            }

            IQueryable<VehicleVersionDto> query = repository.Query()
                .Select(vv => new VehicleVersionDto(
                    vv.Id,
                    vv.Name!,
                    vv.ModelId
                ));

            return await GetPaginatedResultAsync(query, pagination);
        }

 
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(int id)
        {
            VehicleVersion? version = await repository.GetByIdAsync(id);
            if (version is null)
            {
                return NotFound(ApiResponseBuilder.Fail<VehicleVersion>(404, $"Vehicle version with ID {id} not found."));
            }

            return Ok(ApiResponseBuilder.Success(version));
        }

        
        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create([FromBody] VehicleVersion vehicleVersion)
        {
            Domain.Entities.Model? model = await modelRepository.GetByIdAsync(vehicleVersion.ModelId);
            if (model is null)
            {
                return BadRequest(ApiResponseBuilder.Fail<VehicleVersion>(400, $"Model with ID {vehicleVersion.ModelId} does not exist."));
            }

            await repository.Add(vehicleVersion);
            await repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = vehicleVersion.Id },
                ApiResponseBuilder.Success(vehicleVersion, "Vehicle version created successfully."));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleVersion updated)
        {
            if (id != updated.Id)
            {
                return BadRequest(ApiResponseBuilder.Fail<VehicleVersion>(400, "The ID in the URL does not match the ID in the payload."));
            }

            VehicleVersion? existing = await repository.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(ApiResponseBuilder.Fail<VehicleVersion>(404, $"Vehicle version with ID {id} not found."));
            }

            Domain.Entities.Model? model = await modelRepository.GetByIdAsync(updated.ModelId);
            if (model is null)
            {
                return BadRequest(ApiResponseBuilder.Fail<VehicleVersion>(400, $"Model with ID {updated.ModelId} does not exist."));
            }

            existing.Name = updated.Name;
            existing.ModelId = updated.ModelId;

            await repository.UpdateAsync(existing);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(existing, "Vehicle version updated successfully."));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            VehicleVersion? version = await repository.GetByIdAsync(id);
            if (version is null)
            {
                return NotFound(ApiResponseBuilder.Fail<VehicleVersion>(404, $"Vehicle version with ID {id} not found."));
            }

            await repository.DeleteAsync(version);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<VehicleVersion>(null, "Vehicle version deleted successfully."));
        }
    }
}
