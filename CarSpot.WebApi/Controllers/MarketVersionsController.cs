using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketVersionsController : ControllerBase
    {
        private readonly IAuxiliarRepository<MarketVersion> _repository;

        public MarketVersionsController(IAuxiliarRepository<MarketVersion> repository)
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
        public async Task<IActionResult> Create(MarketVersion marketVersion)
        {
            await _repository.Add(marketVersion);
            return CreatedAtAction(nameof(GetById), new { id = marketVersion.Id }, marketVersion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, MarketVersion updated)
        {
            if (id != updated.Id) return BadRequest();
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
