using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult<IEnumerable<Color>>> GetAll()
        {
            var colors = await _repository.GetAllAsync();
            return Ok(colors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> GetById(int id)
        {
            var color = await _repository.GetByIdAsync(id);
            if (color == null)
                return NotFound();

            return Ok(color);
        }

        [HttpPost]
        public async Task<ActionResult<Color>> Create([FromBody] Color color)
        {
            if (color == null || string.IsNullOrWhiteSpace(color.Name))
                return BadRequest("Color data is required and name must not be empty.");

            await _repository.Add(color);
            return CreatedAtAction(nameof(GetById), new { id = color.Id }, color);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Color updated)
        {
            if (updated == null || id != updated.Id)
                return BadRequest("ID mismatch or invalid data.");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = updated.Name;
            await _repository.UpdateAsync(existing);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var color = await _repository.GetByIdAsync(id);
            if (color == null)
                return NotFound($"Color with ID {id} does not exist.");

            await _repository.DeleteAsync(color);
            return NoContent();
        }

    }
}
