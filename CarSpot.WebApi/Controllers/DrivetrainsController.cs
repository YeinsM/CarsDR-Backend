
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
    public class DrivetrainsController(IAuxiliarRepository<Drivetrain> repository, IPaginationService paginationService) : PaginatedControllerBase(paginationService)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<DrivetrainDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            IQueryable<DrivetrainDto> query = repository.Query()
                .Select(d => new DrivetrainDto(
                    d.Id,
                    d.Name
                ));

            return await GetPaginatedResultAsync(query, pagination);
        }





        
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(int id)
        {
            Drivetrain? item = await repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(ApiResponseBuilder.Fail<Drivetrain>(404, $"Drivetrain with ID {id} not found."));
            }

            return Ok(ApiResponseBuilder.Success(item));
        }

        
        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create(Drivetrain drivetrain)
        {
            if (string.IsNullOrWhiteSpace(drivetrain.Name))
            {
                return BadRequest(ApiResponseBuilder.Fail<Drivetrain>(400, "Name is required."));
            }

            await repository.Add(drivetrain);
            await repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = drivetrain.Id },
                ApiResponseBuilder.Success(drivetrain, "Drivetrain created successfully."));
        }


        
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, Drivetrain updated)
        {
            if (id != updated.Id)
            {
                return BadRequest(ApiResponseBuilder.Fail<Drivetrain>(400, "ID in route does not match ID in body."));
            }

            Drivetrain? existing = await repository.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(ApiResponseBuilder.Fail<Drivetrain>(404, $"Drivetrain with ID {id} not found."));
            }

            existing.Name = updated.Name;
            await repository.UpdateAsync(existing);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(existing, "Drivetrain updated successfully."));
        }


        
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            Drivetrain? item = await repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(ApiResponseBuilder.Fail<Drivetrain>(404, $"Drivetrain with ID {id} not found."));
            }

            await repository.DeleteAsync(item);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<Drivetrain>(null, "Drivetrain deleted successfully."));
        }
    }
}
