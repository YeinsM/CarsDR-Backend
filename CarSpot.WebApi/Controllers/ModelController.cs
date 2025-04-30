using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;



namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelController(IRepository<Model> _modelRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var models = await _modelRepository.GetAllAsync();
            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _modelRepository.GetByIdAsync(id);
            if (model == null)
                return NotFound("Model not found.");

            var dto = new ModelDto(model.Id, model.Name, model.MakeId);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateModelRequest request)
        {
            var model = new Model(request.Name, request.MakeId);
            await _modelRepository.AddAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateModelRequest updateRequest)
        {
            var model = await _modelRepository.GetByIdAsync(id);
            if (model is null)
            {
                return NotFound();
            }

            model.Update(updateRequest.Name, updateRequest.MakeId);

            await _modelRepository.UpdateAsync(model);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _modelRepository.GetByIdAsync(id);
            if (model == null)
                return NotFound("Model not found.");

            await _modelRepository.DeleteAsync(model);
            return NoContent();
        }
    }
}
