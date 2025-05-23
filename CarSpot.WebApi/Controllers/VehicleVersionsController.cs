using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleVersionsController : ControllerBase
    {
        private readonly IAuxiliarRepository<VehicleVersion> _repository;
        private readonly IAuxiliarRepository<Model> _modelRepository;

        public VehicleVersionsController(
            IAuxiliarRepository<VehicleVersion> repository,
            IAuxiliarRepository<Model> modelRepository)
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
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleVersion vehicleVersion)
        {
            
        
                var model = await _modelRepository.GetByIdAsync(vehicleVersion.ModelId);
                if (model is null)
                    return BadRequest($"Model with ID {vehicleVersion.ModelId} does not exist.");

            await _repository.Add(vehicleVersion);
            return CreatedAtAction(nameof(GetById), new { id = vehicleVersion.Id }, vehicleVersion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleVersion updated)
        {
            if (id != updated.Id)
                return BadRequest("The ID in the URL does not match the ID in the payload.");
            {
                var model = await _modelRepository.GetByIdAsync(updated.ModelId);
                if (model is null)
                    return BadRequest($"Model with ID {updated.ModelId} does not exist.");
            }

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
