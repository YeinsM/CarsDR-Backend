using System.Linq;
using System.Threading.Tasks;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.Common.Responses;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly IAuxiliarRepository<Country> _repository;

        public CountryController(IAuxiliarRepository<Country> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countries = await _repository.GetAllAsync();
            var response = countries.Select(c => new CountryResponse(c.Id, c.Name, c.Abbreviation));
            return Ok(ApiResponseBuilder.Success(response, "List of countries retrieved successfully."));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var country = await _repository.GetByIdAsync(id);
            return country == null
                ? NotFound(ApiResponseBuilder.Fail<CountryResponse>(404, $"Country with ID {id} not found."))
                : Ok(ApiResponseBuilder.Success(new CountryResponse(country.Id, country.Name, country.Abbreviation)));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCountryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Abbreviation))
                return BadRequest(ApiResponseBuilder.Fail<CountryResponse>(400, "Invalid country data. Name and abbreviation are required."));

            var country = new Country
            {
                Name = request.Name,
                Abbreviation = request.Abbreviation
            };

            await _repository.Add(country);
            await _repository.SaveChangesAsync();

            var response = new CountryResponse(country.Id, country.Name, country.Abbreviation);
            return CreatedAtAction(nameof(GetById), new { id = country.Id }, ApiResponseBuilder.Success(response, "Country created successfully."));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateCountryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Abbreviation))
                return BadRequest(ApiResponseBuilder.Fail<CountryResponse>(400, "Invalid update data. Name and abbreviation are required."));

            var country = await _repository.GetByIdAsync(id);
            if (country == null)
                return NotFound(ApiResponseBuilder.Fail<CountryResponse>(404, $"Country with ID {id} not found."));

            country.Name = request.Name;
            country.Abbreviation = request.Abbreviation;

            await _repository.UpdateAsync(country);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success(new CountryResponse(country.Id, country.Name, country.Abbreviation), "Country updated successfully."));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var country = await _repository.GetByIdAsync(id);
            if (country == null)
                return NotFound(ApiResponseBuilder.Fail<CountryResponse>(404, $"Country with ID {id} not found."));

            await _repository.DeleteAsync(country);
            await _repository.SaveChangesAsync();

            return Ok(ApiResponseBuilder.Success<CountryResponse>(null, "Country deleted successfully."));
        }
    }
}
