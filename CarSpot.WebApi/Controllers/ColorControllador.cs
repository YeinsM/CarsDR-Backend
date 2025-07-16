
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAll()
        {
            var colors = await _repository.GetAllAsync();
            return Ok(ApiResponseBuilder.Success(colors, "Colors retrieved successfully."));
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
