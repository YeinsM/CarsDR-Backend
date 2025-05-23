using System;
using System.Threading.Tasks;
using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Domain.Common;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelsController : ControllerBase
    {
        private readonly IAuxiliarRepository<Model> _repository;
        private readonly IAuxiliarRepository<Make> _makeRepository; 

        public ModelsController(IAuxiliarRepository<Model> repository, IAuxiliarRepository<Make> makeRepository)
        {
            _repository = repository;
            _makeRepository = makeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model model)
        {
            
            var make = await _makeRepository.GetByIdAsync(model.MakeId);
            if (make is null)
                return BadRequest($"Make with ID {model.MakeId} does not exist.");

            await _repository.Add(model);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model updated)
        {
            if (id != updated.Id) return BadRequest();

            
            var make = await _makeRepository.GetByIdAsync(updated.MakeId);
            if (make is null)
                return BadRequest($"Make with ID {updated.MakeId} does not exist.");

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
