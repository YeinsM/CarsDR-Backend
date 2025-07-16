using System;
using System.Threading.Tasks;
using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.Common.Responses;


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
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(ApiResponseBuilder.Success(items, "List of conditions retrieved successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var condition = await _repository.GetByIdAsync(id);
            return condition == null
                ? NotFound(ApiResponseBuilder.Fail<Condition>(404, $"Condition with ID {id} does not exist."))
                : Ok(ApiResponseBuilder.Success(condition));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Condition condition)
        {
            if (condition == null || string.IsNullOrWhiteSpace(condition.Name))
                return BadRequest(ApiResponseBuilder.Fail<Condition>(400, "Invalid condition data."));

            try
            {
                await _repository.Add(condition);
                await _repository.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = condition.Id }, 
                    ApiResponseBuilder.Success(condition, "Condition created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseBuilder.Fail<Condition>(500, $"Internal error: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Condition updated)
        {
            if (id != updated.Id)
                return BadRequest(ApiResponseBuilder.Fail<Condition>(400, "ID mismatch between URL and body."));

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(ApiResponseBuilder.Fail<Condition>(404, $"Condition with ID {id} does not exist."));

            existing.Name = updated.Name;

            try
            {
                await _repository.UpdateAsync(existing);
                await _repository.SaveChangesAsync();
                return Ok(ApiResponseBuilder.Success(existing, "Condition updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseBuilder.Fail<Condition>(500, $"Internal error: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var condition = await _repository.GetByIdAsync(id);
            if (condition == null)
                return NotFound(ApiResponseBuilder.Fail<Condition>(404, $"Condition with ID {id} does not exist."));

            try
            {
                await _repository.DeleteAsync(condition);
                await _repository.SaveChangesAsync();
                return Ok(ApiResponseBuilder.Success<Condition>(null, "Condition deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseBuilder.Fail<Condition>(500, $"Internal error: {ex.Message}"));
            }
        }
    }
}
