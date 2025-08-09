using System;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces;
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
        public async Task<IActionResult> Create(Role role)
        {
            await _repository.CreateAsync(role);
            await _repository.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created,
                ApiResponseBuilder.Success(role, "Role created successfully."));
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Role updated)
        {
            if (id != updated.Id)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ApiResponseBuilder.Fail<object?>(StatusCodes.Status400BadRequest, "URL ID does not match body ID."));
            }

            await _repository.UpdateAsync(updated);

            return StatusCode(StatusCodes.Status204NoContent,
                ApiResponseBuilder.Success<object?>(null, "Role updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role is null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    ApiResponseBuilder.Fail<object?>(StatusCodes.Status404NotFound, $"Role with ID {id} does not exist."));
            }

            await _repository.DeleteAsync(id);

            return StatusCode(StatusCodes.Status204NoContent,
                ApiResponseBuilder.Success<object?>(null, "Role deleted successfully."));
        }

    }
}
