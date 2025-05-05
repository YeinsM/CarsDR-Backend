using Microsoft.AspNetCore.Mvc;
using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using CarSpot.Application.DTOs;
using CarSpot.Application.DTOs.MakeDtos;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MakeController(IRepository<Make> _makeRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Make>>> GetAll()
        {
            var makes = await _makeRepository.GetAllAsync();
            return Ok(makes);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Make>> GetById(Guid id)
        {
            var make = await _makeRepository.GetByIdAsync(id);
            if (make == null) return NotFound();

            return Ok(make);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMakeRequest request)
        {
            var make = new Make(request.Name);
            await _makeRepository.AddAsync(make);
            return CreatedAtAction(nameof(GetById), new { id = make.Id }, make);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMakeRequest updateRequest)
        {
            var make = await _makeRepository.GetByIdAsync(id);
            if (make is null)
            {
                return NotFound();
            }


            make.Update(updateRequest.Name);

            await _makeRepository.UpdateAsync(make);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var make = await _makeRepository.GetByIdAsync(id);
            if (make == null) return NotFound();

            await _makeRepository.DeleteAsync(make);
            return NoContent();
        }
    }
}
