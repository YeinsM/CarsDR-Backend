using System;
using System.Threading.Tasks;
using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.Common.Responses;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CarSpot.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces.Services;
using CarSpot.WebApi.Controllers.Base;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConditionsController : PaginatedControllerBase
    {
        private readonly IAuxiliarRepository<Condition> _repository;

        public ConditionsController(IAuxiliarRepository<Condition> repository, IPaginationService paginationService) 
            : base(paginationService)
        {
            _repository = repository;
        }


        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<ConditionDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            var query = _repository.Query()
                .Select(c => new ConditionDto(c.Id, c.Name));

            return await GetPaginatedResultAsync(query, pagination, "List of conditions retrieved successfully.", true);
        }



        
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(int id)
        {
            var condition = await _repository.GetByIdAsync(id);
            return condition == null
                ? NotFound(ApiResponseBuilder.Fail<Condition>(404, $"Condition with ID {id} does not exist."))
                : Ok(ApiResponseBuilder.Success(condition));
        }


        
        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create(Condition condition)
        {
            if (condition == null || string.IsNullOrWhiteSpace(condition.Name))
    {
        return BadRequest(ApiResponseBuilder.Fail<Condition>(400, "Invalid condition data."));
    }

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
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, Condition updated)
        {
            if (id != updated.Id)
    {
        return BadRequest(ApiResponseBuilder.Fail<Condition>(400, "ID mismatch between URL and body."));
    }

    var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
    {
        return NotFound(ApiResponseBuilder.Fail<Condition>(404, $"Condition with ID {id} does not exist."));
    }

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
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var condition = await _repository.GetByIdAsync(id);
            if (condition == null)
    {
        return NotFound(ApiResponseBuilder.Fail<Condition>(404, $"Condition with ID {id} does not exist."));
    }

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
