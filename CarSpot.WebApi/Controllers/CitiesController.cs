using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly IAuxiliarRepository<City> _cityRepository;

        public CitiesController(IAuxiliarRepository<City> cityRepository)
        {
            _cityRepository = cityRepository;
        }

        [HttpGet]
        public IActionResult GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = _cityRepository.Query();

            var totalRecords = query.Count();
            var cities = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CityResponse(c.Id, c.Name, c.CountryId))
                .ToList();

            var pagedResponse = new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = cities
            };

            return Ok(ApiResponseBuilder.Success(pagedResponse));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, $"City with id {id} not found."));

            var response = new CityResponse(city.Id, city.Name, city.CountryId);
            return Ok(ApiResponseBuilder.Success(response));
        }

        [HttpPost]
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

            var response = new CityResponse(city.Id, city.Name, city.CountryId);

            return CreatedAtAction(nameof(GetById), new { id = city.Id }, ApiResponseBuilder.Success(response, "City created successfully."));
        }

        [HttpPut("{id}")]
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
            return Ok(ApiResponseBuilder.Success<string>(null, "City updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, $"City with id {id} not found."));

            await _cityRepository.DeleteAsync(city);
            return Ok(ApiResponseBuilder.Success<string>(null, "City deleted successfully."));
        }
    }
}
