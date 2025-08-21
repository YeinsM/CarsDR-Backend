using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CarSpot.Application.Interfaces.Services;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CitiesController : ControllerBase
    {
        private readonly IAuxiliarRepository<City> _cityRepository;
        private readonly IPaginationService _paginationService;

        public CitiesController(IAuxiliarRepository<City> cityRepository, IPaginationService paginationService)
        {
            _cityRepository = cityRepository;
            _paginationService = paginationService;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<CityResponse>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            var query = _cityRepository.Query()
                .Select(c => new CityResponse(c.Id, c.Name, c.CountryId));

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
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, $"City with id {id} not found."));

            var response = new CityResponse(city.Id, city.Name, city.CountryId);
            return Ok(ApiResponseBuilder.Success(response));
        }


        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseBuilder.Fail<string>(400, "Invalid request payload."));

            var city = new City
            {
                Name = request.Name,
                CountryId = request.CountryId
            };

            await _cityRepository.Add(city);
            await _cityRepository.SaveChangesAsync();

            var response = new CityResponse(city.Id, city.Name, city.CountryId);

            return CreatedAtAction(nameof(GetById), new { id = city.Id }, ApiResponseBuilder.Success(response, "City created successfully."));
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseBuilder.Fail<string>(400, "Invalid request payload."));

            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, $"City with id {id} not found."));

            city.Name = request.Name;
            city.CountryId = request.CountryId;

            await _cityRepository.UpdateAsync(city);
            await _cityRepository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "City updated successfully."));
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, $"City with id {id} not found."));

            await _cityRepository.DeleteAsync(city);
            await _cityRepository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "City deleted successfully."));
        }
    }
}
