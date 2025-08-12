
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrivetrainsController : ControllerBase
    {
        private readonly IAuxiliarRepository<Drivetrain> _repository;
        private readonly IPaginationService _paginationService;

        public DrivetrainsController(IAuxiliarRepository<Drivetrain> repository, IPaginationService paginationService)
        {
            _repository = repository;
            _paginationService = paginationService;
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<DrivetrainDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            var query = _repository.Query();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query.Select(d => new DrivetrainDto(
                    d.Id,
                    d.Name
                )),
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }





        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                return NotFound(ApiResponseBuilder.Fail<Drivetrain>(404, $"Drivetrain with ID {id} not found."));

            return Ok(ApiResponseBuilder.Success(item));
        }

        [Authorize(Policy = "AdminOrCompany")]
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


        [Authorize(Policy = "AdminOnly")]
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


        [Authorize(Policy = "AdminOnly")]
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
