using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CabTypesController : ControllerBase
    {
        private readonly IAuxiliarRepository<CabType> _repository;

        public CabTypesController(IAuxiliarRepository<CabType> repository)
        {
            _repository = repository;
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
        public async Task<IActionResult> Create(CabType cabType)
        {
            await _repository.Add(cabType);
            return CreatedAtAction(nameof(GetById), new { id = cabType.Id }, cabType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CabType updated)
        {
            if (id != updated.Id) return BadRequest();
            await _repository.UpdateAsync(updated);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
