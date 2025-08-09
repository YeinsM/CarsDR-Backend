using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransmissionsController : ControllerBase
    {
        private readonly IAuxiliarRepository<Transmission> _repository;

        public TransmissionsController(IAuxiliarRepository<Transmission> repository)
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

            var query = _repository.Query();

            var totalItems = await query.CountAsync();

            var transmissions = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize),
                Items = transmissions
            };

            return Ok(ApiResponseBuilder.Success(response, "List of transmissions retrieved successfully."));
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transmission = await _repository.GetByIdAsync(id);
            if (transmission is null)
                return NotFound(ApiResponseBuilder.Fail<Transmission>(404, $"Transmission with ID {id} not found."));

            return Ok(ApiResponseBuilder.Success(transmission));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransmissionRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(ApiResponseBuilder.Fail<Transmission>(400, "Invalid transmission name."));

            var transmission = new Transmission { Name = request.Name };
            await _repository.Add(transmission);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = transmission.Id },
                ApiResponseBuilder.Success(transmission, "Transmission created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTransmissionRequest request)
        {
            if (!ModelState.IsValid || id != request.Id)
                return BadRequest(ApiResponseBuilder.Fail<Transmission>(400, "Invalid data or mismatched IDs."));

            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
                return NotFound(ApiResponseBuilder.Fail<Transmission>(404, $"Transmission with ID {id} not found."));

            existing.Name = request.Name;
            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(existing, "Transmission updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transmission = await _repository.GetByIdAsync(id);
            if (transmission is null)
                return NotFound(ApiResponseBuilder.Fail<Transmission>(404, $"Transmission with ID {id} not found."));

            await _repository.DeleteAsync(transmission);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<Transmission>(null, "Transmission deleted successfully."));
        }
    }
}
