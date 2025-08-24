using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ColorsController(IAuxiliarRepository<Color> repository, IPaginationService paginationService) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<ColorResponse>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            IQueryable<ColorResponse> query = repository.Query()
                .Select(c => new ColorResponse
                    {
                        Id = c.Id,
                        Name = c.Name
                    });

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            PaginatedResponse<ColorResponse> paginatedResult = await paginationService.PaginateAsync(
                query,
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(ApiResponseBuilder.Success(paginatedResult, "Colors retrieved successfully."));
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            Color? color = await repository.GetByIdAsync(id);
            if (color is null)
            {
                return NotFound(ApiResponseBuilder.Fail<Color>(404, $"Color with ID {id} not found."));
            }

            return Ok(ApiResponseBuilder.Success(color, "Color retrieved successfully."));
        }


        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] Color color)
        {
            if (color is null || string.IsNullOrWhiteSpace(color.Name))
            {
                return BadRequest(ApiResponseBuilder.Fail<Color>(400, "Color data is required and name must not be empty."));
            }

            await repository.Add(color);
            await repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = color.Id }, ApiResponseBuilder.Success(color, "Color created successfully."));
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] Color updated)
        {
            if (updated is null || id != updated.Id)
            {
                return BadRequest(ApiResponseBuilder.Fail<Color>(400, "ID mismatch or invalid data."));
            }

            Color? existing = await repository.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(ApiResponseBuilder.Fail<Color>(404, $"Color with ID {id} not found."));
            }

            existing.Name = updated.Name;
            await repository.UpdateAsync(existing);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(existing, "Color updated successfully."));
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            Color? color = await repository.GetByIdAsync(id);
            if (color is null)
            {
                return NotFound(ApiResponseBuilder.Fail<Color>(404, $"Color with ID {id} not found."));
            }

            await repository.DeleteAsync(color);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(color, "Color deleted successfully."));
        }
    }
}
