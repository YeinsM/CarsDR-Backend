using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionsController : ControllerBase
    {
        private readonly IAuxiliarRepository<Version> _repository;
        private readonly IAuxiliarRepository<Model> _modelRepository;

        public VersionsController(IAuxiliarRepository<Version> repository, IAuxiliarRepository<Model> modelRepository)
        {
            _repository = repository;
            _modelRepository = modelRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Version version)
        {
            
            if (version.ModelId.HasValue)
            {
                var model = await _modelRepository.GetByIdAsync(version.ModelId.Value);
                if (model is null)
                    return BadRequest($"Model with ID {version.ModelId} does not exist.");
            }

            await _repository.Add(version);
            return CreatedAtAction(nameof(GetById), new { id = version.Id }, version);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Version updated)
        {
            if (id != updated.Id) return BadRequest();

            
            if (updated.ModelId.HasValue)
            {
                var model = await _modelRepository.GetByIdAsync(updated.ModelId.Value);
                if (model is null)
                    return BadRequest($"Model with ID {updated.ModelId} does not exist.");
            }

            await _repository.UpdateAsync(updated);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
