using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.Common.Responses;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrivetrainsController : ControllerBase
    {
        private readonly IAuxiliarRepository<Drivetrain> _repository;

        public DrivetrainsController(IAuxiliarRepository<Drivetrain> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            const int maxPageSize = 100;

            if (pageNumber <= 0)
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "Page number must be greater than zero."));

            if (pageSize <= 0)
                pageSize = 1;
            else if (pageSize > maxPageSize)
                pageSize = maxPageSize;

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

            return Ok(ApiResponseBuilder.Success(paginatedResponse, "List of drivetrains retrieved successfully."));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                return NotFound(ApiResponseBuilder.Fail<Drivetrain>(404, $"Drivetrain with ID {id} not found."));

            return Ok(ApiResponseBuilder.Success(item));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Drivetrain drivetrain)
        {
            if (string.IsNullOrWhiteSpace(drivetrain.Name))
                return BadRequest(ApiResponseBuilder.Fail<Drivetrain>(400, "Name is required."));

            await _repository.Add(drivetrain);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = drivetrain.Id },
                ApiResponseBuilder.Success(drivetrain, "Drivetrain created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Drivetrain updated)
        {
            if (id != updated.Id)
                return BadRequest(ApiResponseBuilder.Fail<Drivetrain>(400, "ID in route does not match ID in body."));

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(ApiResponseBuilder.Fail<Drivetrain>(404, $"Drivetrain with ID {id} not found."));

            existing.Name = updated.Name;
            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(existing, "Drivetrain updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                return NotFound(ApiResponseBuilder.Fail<Drivetrain>(404, $"Drivetrain with ID {id} not found."));

            await _repository.DeleteAsync(item);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<Drivetrain>(null, "Drivetrain deleted successfully."));
        }
    }
}
