using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using CarSpot.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CitiesController(IAuxiliarRepository<City> cityRepository, IPaginationService paginationService) : PaginatedControllerBase(paginationService)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<CityResponse>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            IQueryable<CityResponse> query = cityRepository.Query()
                .Select(c => new CityResponse(c.Id, c.Name, c.CountryId));

            return await GetPaginatedResultAsync(query, pagination, useApiResponseBuilder: true);
        }



        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            City? city = await cityRepository.GetByIdAsync(id);
            if (city == null)
            {
                return NotFound(ApiResponseBuilder.Fail<string>(404, $"City with id {id} not found."));
            }

            var response = new CityResponse(city.Id, city.Name, city.CountryId);
            return Ok(ApiResponseBuilder.Success(response));
        }


        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCityRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseBuilder.Fail<string>(400, "Invalid request payload."));
            }

            var city = new City
            {
                Name = request.Name,
                CountryId = request.CountryId
            };

            await cityRepository.Add(city);
            await cityRepository.SaveChangesAsync();

            var response = new CityResponse(city.Id, city.Name, city.CountryId);

            return CreatedAtAction(nameof(GetById), new { id = city.Id }, ApiResponseBuilder.Success(response, "City created successfully."));
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCityRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseBuilder.Fail<string>(400, "Invalid request payload."));
            }

            City? city = await cityRepository.GetByIdAsync(id);
            if (city == null)
            {
                return NotFound(ApiResponseBuilder.Fail<string>(404, $"City with id {id} not found."));
            }

            city.Name = request.Name;
            city.CountryId = request.CountryId;

            await cityRepository.UpdateAsync(city);
            await cityRepository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "City updated successfully."));
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            City? city = await cityRepository.GetByIdAsync(id);
            if (city == null)
            {
                return NotFound(ApiResponseBuilder.Fail<string>(404, $"City with id {id} not found."));
            }

            await cityRepository.DeleteAsync(city);
            await cityRepository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<string>(null, "City deleted successfully."));
        }
    }
}
