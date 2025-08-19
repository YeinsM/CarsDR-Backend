using System;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _repository;

        public RolesController(IRoleRepository repository)
        {
            _repository = repository;
        }

     
        [HttpGet]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(ApiResponseBuilder.Success(items, "Roles retrieved successfully."));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item is null
                ? NotFound(ApiResponseBuilder.Fail<object?>(StatusCodes.Status404NotFound, $"Role with ID {id} not found."))
                : Ok(ApiResponseBuilder.Success(item, "Role retrieved successfully."));
        }


        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] Role role)
        {
            if (role is null || string.IsNullOrWhiteSpace(role.Description))
            {
                return BadRequest(ApiResponseBuilder.Fail<object?>(StatusCodes.Status400BadRequest, "Invalid role data."));
            }

            await _repository.CreateAsync(role);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = role.Id },
                ApiResponseBuilder.Success(role, "Role created successfully."));
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Role updated)
        {
            if (id != updated.Id)
            {
                return BadRequest(ApiResponseBuilder.Fail<object?>(StatusCodes.Status400BadRequest, "URL ID does not match body ID."));
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(ApiResponseBuilder.Fail<object?>(StatusCodes.Status404NotFound, $"Role with ID {id} not found."));
            }

            await _repository.UpdateAsync(updated);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(updated, "Role updated successfully."));
        }

       
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role is null)
            {
                return NotFound(ApiResponseBuilder.Fail<object?>(StatusCodes.Status404NotFound, $"Role with ID {id} not found."));
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<object?>(null, "Role deleted successfully."));
        }
    }
}
