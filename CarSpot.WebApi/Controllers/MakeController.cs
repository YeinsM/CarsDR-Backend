using System;
using System.Threading.Tasks;
using CarSpot.Application.DTOs.MakeDtos;
using CarSpot.Application.Interfaces;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MakesController : ControllerBase
    {
        private readonly IMakeRepository _repository;

        public MakesController(IMakeRepository repository)
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
        public async Task<IActionResult> Create([FromBody] CreateMakeRequest request)
        {
            var make = new Make(request.Name);
            await _repository.Add(make);
            return CreatedAtAction(nameof(GetById), new { id = make.Id }, make);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] string newName)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return NotFound();

            existing.Update(newName);
            await _repository.UpdateAsync(id, newName);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.RemoveAsync(id);
            return NoContent();
        }
    }
}
