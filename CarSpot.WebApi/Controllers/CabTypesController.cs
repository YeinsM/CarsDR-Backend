using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CabTypesController(
        IAuxiliarRepository<CabType> repository,
        IPaginationService paginationService) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<CabTypeDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            System.Linq.IQueryable<CabType> query = repository.Query();

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            PaginatedResponse<CabType> paginatedResult = await paginationService.PaginateAsync(
                query,
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(ApiResponseBuilder.Success(paginatedResult));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(int id)
        {
            CabType? item = await repository.GetByIdAsync(id);

            if (item is null)
            {
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Item not found."));
            }

            return Ok(ApiResponseBuilder.Success(item, "Item found successfully."));
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CabType cabType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseBuilder.Fail<string>(400, "Invalid input."));
            }

            await repository.Add(cabType);
            await repository.SaveChangesAsync();

            ApiResponse<CabType> response = ApiResponseBuilder.Success(cabType, "CabType created successfully.");

            return CreatedAtAction(nameof(GetById), new { id = cabType.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, CabType updated)
        {
            if (id != updated.Id)
            {
                return BadRequest(ApiResponseBuilder.Fail<string>(400, "The ID in the URL does not match the request body."));
            }

            CabType? existing = await repository.GetByIdAsync(id);

            if (existing is null)
            {
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Cab type not found."));
            }

            existing.Name = updated.Name;
            await repository.UpdateAsync(existing);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "Cab type updated successfully."));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            CabType? cabType = await repository.GetByIdAsync(id);

            if (cabType is null)
            {
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Cab type not found."));
            }

            await repository.DeleteAsync(cabType);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "Cab type deleted successfully."));
        }
    }
}
