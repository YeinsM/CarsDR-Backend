using System;
using System.Threading.Tasks;

using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConditionsController : ControllerBase
    {
        private readonly IAuxiliarRepository<Condition> _repository;

        public ConditionsController(IAuxiliarRepository<Condition> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async
        Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var condition = await _repository.GetByIdAsync(id);
            if (condition == null)
                return NotFound();
            return Ok(condition);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Condition condition)
        {
            try
            {
                await _repository.Add(condition);
                await _repository.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = condition.Id }, condition);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Condition updated)
        {
            if (id != updated.Id) return BadRequest();
            await _repository.UpdateAsync(updated);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
