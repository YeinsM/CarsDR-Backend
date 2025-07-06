using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers;

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
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetAllAsync()
    {
        var cities = await _cityRepository.GetAllAsync();
        var response = cities.Select(c => new CityResponse(c.Id, c.Name, c.CountryId));
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CityResponse>> GetById(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null) return NotFound();
        return Ok(new CityResponse(city.Id, city.Name, city.CountryId));
    }

    [HttpPost]
    public async Task<ActionResult<CityResponse>> CreateAsync(CreateCityRequest request)
    {
        var city = new City { Name = request.Name, CountryId = request.CountryId };
        await _cityRepository.Add(city);
        return CreatedAtAction(nameof(GetById),"Cities", new { id = city.Id }, new CityResponse(city.Id, city.Name, city.CountryId));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateCityRequest request)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null) return NotFound();

        city.Name = request.Name;
        city.CountryId = request.CountryId;

        await _cityRepository.UpdateAsync(city);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null) return NotFound();

        await _cityRepository.DeleteAsync(id);
        return NoContent();
    }
}
