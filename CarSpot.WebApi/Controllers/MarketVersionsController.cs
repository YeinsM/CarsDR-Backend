using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.Common.Responses;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketVersionsController : ControllerBase
    {
        private readonly IAuxiliarRepository<MarketVersion> _repository;

        public MarketVersionsController(IAuxiliarRepository<MarketVersion> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(ApiResponseBuilder.Success(items, "List of market versions retrieved successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item is null)
                return NotFound(ApiResponseBuilder.Fail<MarketVersion>(404, $"Market version with ID {id} not found."));

            return Ok(ApiResponseBuilder.Success(item));
        }

        [HttpPost]
        public async Task<IActionResult> Create(MarketVersion marketVersion)
        {
            if (string.IsNullOrWhiteSpace(marketVersion.Name))
                return BadRequest(ApiResponseBuilder.Fail<MarketVersion>(400, "Name is required."));

            await _repository.Add(marketVersion);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = marketVersion.Id },
                ApiResponseBuilder.Success(marketVersion, "Market version created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MarketVersion updated)
        {
            if (id != updated.Id)
                return BadRequest(ApiResponseBuilder.Fail<MarketVersion>(400, "ID in route does not match ID in body."));

            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
                return NotFound(ApiResponseBuilder.Fail<MarketVersion>(404, $"Market version with ID {id} not found."));

            existing.Name = updated.Name;
            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(existing, "Market version updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item is null)
                return NotFound(ApiResponseBuilder.Fail<MarketVersion>(404, $"Market version with ID {id} not found."));

            await _repository.DeleteAsync(item);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<MarketVersion>(null, "Market version deleted successfully."));
        }
    }
}
