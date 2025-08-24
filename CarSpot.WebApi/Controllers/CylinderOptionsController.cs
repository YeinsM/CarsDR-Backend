
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using CarSpot.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CylinderOptionsController(IAuxiliarRepository<CylinderOption> repository, IPaginationService paginationService) : PaginatedControllerBase(paginationService)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<CylinderOptionDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            IQueryable<CylinderOptionDto> query = repository.Query()
                .Select(c => new CylinderOptionDto(
                    c.Id,
                    c.Name
                ));

            return await GetPaginatedResultAsync(query, pagination);
        }


        
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(int id)
        {
            CylinderOption? item = await repository.GetByIdAsync(id);
            if (item is null)
            {
                return NotFound(ApiResponseBuilder.Fail<CylinderOption>(404, $"Cylinder option with ID {id} not found."));
            }

            return Ok(ApiResponseBuilder.Success(item));
        }


        
        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create(CylinderOption cylinderOption)
        {
            if (string.IsNullOrWhiteSpace(cylinderOption.Name))
            {
                return BadRequest(ApiResponseBuilder.Fail<CylinderOption>(400, "Name is required."));
            }

            await repository.Add(cylinderOption);
            await repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = cylinderOption.Id },
                ApiResponseBuilder.Success(cylinderOption, "Cylinder option created successfully."));
        }


        
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, CylinderOption updated)
        {
            if (id != updated.Id)
            {
                return BadRequest(ApiResponseBuilder.Fail<CylinderOption>(400, "ID in route does not match ID in body."));
            }

            CylinderOption? existing = await repository.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(ApiResponseBuilder.Fail<CylinderOption>(404, $"Cylinder option with ID {id} not found."));
            }

            existing.Name = updated.Name;
            await repository.UpdateAsync(existing);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(existing, "Cylinder option updated successfully."));
        }


        
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            CylinderOption? item = await repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(ApiResponseBuilder.Fail<CylinderOption>(404, $"Cylinder option with ID {id} not found."));
            }

            await repository.DeleteAsync(item);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<CylinderOption>(null, "Cylinder option deleted successfully."));
        }
    }
}
