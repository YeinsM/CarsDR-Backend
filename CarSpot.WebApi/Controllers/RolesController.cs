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
    public class RolesController(IRoleRepository repository) : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetAll()
        {
            System.Collections.Generic.IEnumerable<Role> items = await repository.GetAllAsync();
            return Ok(ApiResponseBuilder.Success(items, "Roles retrieved successfully."));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Role? item = await repository.GetByIdAsync(id);
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

            await repository.CreateAsync(role);
            await repository.SaveChangesAsync();

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

            Role? existing = await repository.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(ApiResponseBuilder.Fail<object?>(StatusCodes.Status404NotFound, $"Role with ID {id} not found."));
            }

            await repository.UpdateAsync(updated);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(updated, "Role updated successfully."));
        }

       
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Role? role = await repository.GetByIdAsync(id);
            if (role is null)
            {
                return NotFound(ApiResponseBuilder.Fail<object?>(StatusCodes.Status404NotFound, $"Role with ID {id} not found."));
            }

            await repository.DeleteAsync(id);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<object?>(null, "Role deleted successfully."));
        }
    }
}
