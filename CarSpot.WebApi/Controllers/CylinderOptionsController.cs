using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.Common.Responses;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CylinderOptionsController : ControllerBase
    {
        private readonly IAuxiliarRepository<CylinderOption> _repository;

        public CylinderOptionsController(IAuxiliarRepository<CylinderOption> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "Page number and page size must be greater than zero."));

            var allItems = await _repository.GetAllAsync();
            var totalItems = allItems.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = allItems
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var paginatedResponse = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            };

            return Ok(ApiResponseBuilder.Success(paginatedResponse, "List of cylinder options retrieved successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item is null)
                return NotFound(ApiResponseBuilder.Fail<CylinderOption>(404, $"Cylinder option with ID {id} not found."));

            return Ok(ApiResponseBuilder.Success(item));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CylinderOption cylinderOption)
        {
            if (string.IsNullOrWhiteSpace(cylinderOption.Name))
                return BadRequest(ApiResponseBuilder.Fail<CylinderOption>(400, "Name is required."));

            await _repository.Add(cylinderOption);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = cylinderOption.Id },
                ApiResponseBuilder.Success(cylinderOption, "Cylinder option created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CylinderOption updated)
        {
            if (id != updated.Id)
                return BadRequest(ApiResponseBuilder.Fail<CylinderOption>(400, "ID in route does not match ID in body."));

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(ApiResponseBuilder.Fail<CylinderOption>(404, $"Cylinder option with ID {id} not found."));

            existing.Name = updated.Name;
            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(existing, "Cylinder option updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                return NotFound(ApiResponseBuilder.Fail<CylinderOption>(404, $"Cylinder option with ID {id} not found."));

            await _repository.DeleteAsync(item);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<CylinderOption>(null, "Cylinder option deleted successfully."));
        }
    }
}
