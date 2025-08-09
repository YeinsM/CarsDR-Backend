using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColorsController : ControllerBase
    {
        private readonly IAuxiliarRepository<Color> _repository;

        public ColorsController(IAuxiliarRepository<Color> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            const int maxPageSize = 100;

            if (pageNumber <= 0)
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "Page number must be greater than zero."));

            if (pageSize <= 0)
                pageSize = 1;
            else if (pageSize > maxPageSize)
                pageSize = maxPageSize;

            var query = _repository.Query();

            var totalRecords = await query.CountAsync();

            var colors = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedResponse = new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = colors
            };

            return Ok(ApiResponseBuilder.Success(pagedResponse, "Colors retrieved successfully."));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var color = await _repository.GetByIdAsync(id);
            if (color is null)
                return NotFound(ApiResponseBuilder.Fail<Color>(404, $"Color with ID {id} not found."));

            return Ok(ApiResponseBuilder.Success(color, "Color retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Color color)
        {
            if (color is null || string.IsNullOrWhiteSpace(color.Name))
                return BadRequest(ApiResponseBuilder.Fail<Color>(400, "Color data is required and name must not be empty."));

            await _repository.Add(color);
            return CreatedAtAction(nameof(GetById), new { id = color.Id }, ApiResponseBuilder.Success(color, "Color created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Color updated)
        {
            if (updated is null || id != updated.Id)
                return BadRequest(ApiResponseBuilder.Fail<Color>(400, "ID mismatch or invalid data."));

            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
                return NotFound(ApiResponseBuilder.Fail<Color>(404, $"Color with ID {id} not found."));

            existing.Name = updated.Name;
            await _repository.UpdateAsync(existing);

            return Ok(ApiResponseBuilder.Success(existing, "Color updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var color = await _repository.GetByIdAsync(id);
            if (color is null)
                return NotFound(ApiResponseBuilder.Fail<Color>(404, $"Color with ID {id} not found."));

            await _repository.DeleteAsync(color);
            return Ok(ApiResponseBuilder.Success(color, "Color deleted successfully."));
        }
    }
}
