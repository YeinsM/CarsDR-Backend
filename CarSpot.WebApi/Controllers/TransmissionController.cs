
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAll()
        {
            var transmissions = await _repository.GetAllAsync();
            return Ok(ApiResponseBuilder.Success(transmissions, "List of transmissions retrieved successfully."));
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
