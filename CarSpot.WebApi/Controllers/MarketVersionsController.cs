
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketVersionsController : ControllerBase
    {
        private readonly IAuxiliarRepository<MarketVersion> _repository;
        private readonly IPaginationService _paginationService;

        public MarketVersionsController(IAuxiliarRepository<MarketVersion> repository, IPaginationService paginationService)
        {
            _repository = repository;
            _paginationService = paginationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<MarketVersionDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            var query = _repository.Query();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query.Select(mv => new MarketVersionDto(
                    mv.Id,
                    mv.Name!
                )),
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }



        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item is null)
                return NotFound(ApiResponseBuilder.Fail<MarketVersion>(404, $"Market version with ID {id} not found."));

            return Ok(ApiResponseBuilder.Success(item));
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create([FromBody] MarketVersion marketVersion)
        {
            if (string.IsNullOrWhiteSpace(marketVersion.Name))
                return BadRequest(ApiResponseBuilder.Fail<MarketVersion>(400, "Name is required."));

            await _repository.Add(marketVersion);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = marketVersion.Id },
                ApiResponseBuilder.Success(marketVersion, "Market version created successfully."));
        }


        [HttpPut("{id}")]
         [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] MarketVersion updated)
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
       [Authorize(Policy = "AdminOnly")]
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
