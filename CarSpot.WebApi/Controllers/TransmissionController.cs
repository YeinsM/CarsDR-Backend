
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
    public class TransmissionsController(IAuxiliarRepository<Transmission> repository, IPaginationService paginationService) : PaginatedControllerBase(paginationService)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<TransmissionDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            IQueryable<TransmissionDto> query = repository.Query()
                .Select(t => new TransmissionDto(
                    t.Id,
                    t.Name
                ));

            return await GetPaginatedResultAsync(query, pagination);
        }



        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(int id)
        {
            Transmission? transmission = await repository.GetByIdAsync(id);
            if (transmission is null)
            {
                return NotFound(ApiResponseBuilder.Fail<Transmission>(404, $"Transmission with ID {id} not found."));
            }

            return Ok(ApiResponseBuilder.Success(transmission));
        }


        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]

        public async Task<IActionResult> Create([FromBody] CreateTransmissionRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(ApiResponseBuilder.Fail<Transmission>(400, "Invalid transmission name."));
            }

            var transmission = new Transmission { Name = request.Name };
            await repository.Add(transmission);
            await repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = transmission.Id },
                ApiResponseBuilder.Success(transmission, "Transmission created successfully."));
        }


        [HttpPut("{id}")]
         [Authorize(Policy = "AdminOnly")]

        public async Task<IActionResult> Update(int id, [FromBody] UpdateTransmissionRequest request)
        {
            if (!ModelState.IsValid || id != request.Id)
            {
                return BadRequest(ApiResponseBuilder.Fail<Transmission>(400, "Invalid data or mismatched IDs."));
            }

            Transmission? existing = await repository.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(ApiResponseBuilder.Fail<Transmission>(404, $"Transmission with ID {id} not found."));
            }

            existing.Name = request.Name;
            await repository.UpdateAsync(existing);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(existing, "Transmission updated successfully."));
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            Transmission? transmission = await repository.GetByIdAsync(id);
            if (transmission is null)
            {
                return NotFound(ApiResponseBuilder.Fail<Transmission>(404, $"Transmission with ID {id} not found."));
            }

            await repository.DeleteAsync(transmission);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<Transmission>(null, "Transmission deleted successfully."));
        }
    }
}
