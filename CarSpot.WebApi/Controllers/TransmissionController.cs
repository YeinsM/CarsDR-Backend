
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CarSpot.Application.Interfaces.Services;
using System.Linq;
using CarSpot.Domain.Common;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransmissionsController : ControllerBase
    {
        private readonly IAuxiliarRepository<Transmission> _repository;
        private readonly IPaginationService _paginationService;

        public TransmissionsController(IAuxiliarRepository<Transmission> repository, IPaginationService paginationService)
        {
            _repository = repository;
            _paginationService = paginationService;
        }


        [HttpGet]
        [Authorize(Policy = "AdminOrCompany")]
        public async Task<ActionResult<PaginatedResponse<TransmissionDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            var query = _repository.Query();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query.Select(t => new TransmissionDto(
                    t.Id,
                    t.Name
                )),
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }



        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrCompany")]
        public async Task<IActionResult> GetById(int id)
        {
            var transmission = await _repository.GetByIdAsync(id);
            if (transmission is null)
                return NotFound(ApiResponseBuilder.Fail<Transmission>(404, $"Transmission with ID {id} not found."));

            return Ok(ApiResponseBuilder.Success(transmission));
        }


        [HttpPost]
        [Authorize(Policy = "AdminOrCompany")]
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
        [Authorize(Policy = "AdminOnly")]
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
        [Authorize(Policy = "AdminOnly")]
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
