using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces.Services;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CabTypesController : ControllerBase
    {
        private readonly IAuxiliarRepository<CabType> _repository;
        private readonly IPaginationService _paginationService;

        public CabTypesController(
            IAuxiliarRepository<CabType> repository,
            IPaginationService paginationService)
        {
            _repository = repository;
            _paginationService = paginationService;
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

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query,
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(ApiResponseBuilder.Success(paginatedResult));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Item not found."));

            return Ok(ApiResponseBuilder.Success(item, "Item found successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CabType cabType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseBuilder.Fail<string>(400, "Invalid input."));

            await _repository.Add(cabType);

            var response = ApiResponseBuilder.Success(cabType, "CabType created successfully.");

            return CreatedAtAction(nameof(GetById), new { id = cabType.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CabType updated)
        {
            if (id != updated.Id)
                return BadRequest(ApiResponseBuilder.Fail<string>(400, "The ID in the URL does not match the request body."));

            var existing = await _repository.GetByIdAsync(id);

            if (existing is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Cab type not found."));

            existing.Name = updated.Name;
            await _repository.UpdateAsync(existing);

            return Ok(ApiResponseBuilder.Success<string>(null, "Cab type updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cabType = await _repository.GetByIdAsync(id);

            if (cabType is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Cab type not found."));

            await _repository.DeleteAsync(cabType);

            return Ok(ApiResponseBuilder.Success<string>(null, "Cab type deleted successfully."));
        }
    }
}
