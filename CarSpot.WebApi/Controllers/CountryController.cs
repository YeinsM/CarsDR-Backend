
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using CarSpot.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController(IAuxiliarRepository<Country> repository, IPaginationService paginationService) : PaginatedControllerBase(paginationService)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<CountryResponse>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            IQueryable<CountryResponse> query = repository.Query()
                .Select(c => new CountryResponse(c.Id, c.Name, c.Abbreviation));

            return await GetPaginatedResultAsync(query, pagination, "List of countries retrieved successfully.", true);
        }



        
        [HttpGet("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetById(int id)
        {
            Country? country = await repository.GetByIdAsync(id);
            return country == null
                ? NotFound(ApiResponseBuilder.Fail<CountryResponse>(404, $"Country with ID {id} not found."))
                : Ok(ApiResponseBuilder.Success(new CountryResponse(country.Id, country.Name, country.Abbreviation)));
        }


        
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create(CreateCountryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Abbreviation))
            {
                return BadRequest(ApiResponseBuilder.Fail<CountryResponse>(400, "Invalid country data. Name and abbreviation are required."));
            }

            var country = new Country
            {
                Name = request.Name,
                Abbreviation = request.Abbreviation
            };

            await repository.Add(country);
            await repository.SaveChangesAsync();

            var response = new CountryResponse(country.Id, country.Name, country.Abbreviation);
            return CreatedAtAction(nameof(GetById), new { id = country.Id }, ApiResponseBuilder.Success(response, "Country created successfully."));
        }


        
        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, UpdateCountryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Abbreviation))
            {
                return BadRequest(ApiResponseBuilder.Fail<CountryResponse>(400, "Invalid update data. Name and abbreviation are required."));
            }

            Country? country = await repository.GetByIdAsync(id);
            if (country == null)
            {
                return NotFound(ApiResponseBuilder.Fail<CountryResponse>(404, $"Country with ID {id} not found."));
            }

            country.Name = request.Name;
            country.Abbreviation = request.Abbreviation;

            await repository.UpdateAsync(country);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(new CountryResponse(country.Id, country.Name, country.Abbreviation), "Country updated successfully."));
        }

       
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            Country? country = await repository.GetByIdAsync(id);
            if (country == null)
            {
                return NotFound(ApiResponseBuilder.Fail<CountryResponse>(404, $"Country with ID {id} not found."));
            }

            await repository.DeleteAsync(country);
            await repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<CountryResponse>(null, "Country deleted successfully."));
        }
    }
}
