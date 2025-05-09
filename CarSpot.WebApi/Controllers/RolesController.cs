using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IAuxiliarRepository<Role> _repository;

        public RolesController(IAuxiliarRepository<Role> repository)
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
        public async Task<IActionResult> Create(Role role)
        {
            await _repository.Add(role);
            return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Role updated)
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
