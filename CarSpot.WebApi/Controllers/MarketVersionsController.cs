
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using CarSpot.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketVersionsController(IAuxiliarRepository<MarketVersion> repository, IPaginationService paginationService) : PaginatedControllerBase(paginationService)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<MarketVersionDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            IQueryable<MarketVersionDto> query = repository.Query()
                .Select(mv => new MarketVersionDto(
                    mv.Id,
                    mv.Name!
                ));

            return await GetPaginatedResultAsync(query, pagination);
        }



        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(int id)
        {
            MarketVersion? item = await repository.GetByIdAsync(id);
            if (item is null)
            {
                return NotFound(ApiResponseBuilder.Fail<MarketVersion>(404, $"Market version with ID {id} not found."));
            }

            return Ok(ApiResponseBuilder.Success(item));
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create([FromBody] MarketVersion marketVersion)
        {
            if (string.IsNullOrWhiteSpace(marketVersion.Name))
            {
                return BadRequest(ApiResponseBuilder.Fail<MarketVersion>(400, "Name is required."));
            }

            await repository.Add(marketVersion);
            await repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = marketVersion.Id },
                ApiResponseBuilder.Success(marketVersion, "Market version created successfully."));
        }


        [HttpPut("{id}")]
         [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] MarketVersion updated)
        {
            if (id != updated.Id)
            {
                return BadRequest(ApiResponseBuilder.Fail<MarketVersion>(400, "ID in route does not match ID in body."));
            }

            MarketVersion? existing = await repository.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(ApiResponseBuilder.Fail<MarketVersion>(404, $"Market version with ID {id} not found."));
            }

            existing.Name = updated.Name;
            await repository.UpdateAsync(existing);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(existing, "Market version updated successfully."));
        }

        [HttpDelete("{id}")]
       [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            MarketVersion? item = await repository.GetByIdAsync(id);
            if (item is null)
            {
                return NotFound(ApiResponseBuilder.Fail<MarketVersion>(404, $"Market version with ID {id} not found."));
            }

            await repository.DeleteAsync(item);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<MarketVersion>(null, "Market version deleted successfully."));
        }
    }
}
