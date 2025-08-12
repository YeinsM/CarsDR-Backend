
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
    public class CylinderOptionsController : ControllerBase
    {
        private readonly IAuxiliarRepository<CylinderOption> _repository;
        private readonly IPaginationService _paginationService;

        public CylinderOptionsController(IAuxiliarRepository<CylinderOption> repository, IPaginationService paginationService)
        {
            _repository = repository;
            _paginationService = paginationService;
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<CylinderOptionDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            var query = _repository.Query();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query.Select(c => new CylinderOptionDto(
                    c.Id,
                    c.Name
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
            if (item is null)
                return NotFound(ApiResponseBuilder.Fail<CylinderOption>(404, $"Cylinder option with ID {id} not found."));

            return Ok(ApiResponseBuilder.Success(item));
        }


        [Authorize(Policy = "AdminOrCompany")]
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


        [Authorize(Policy = "AdminOnly")]
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


        [Authorize(Policy = "AdminOnly")]
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
