using System.Threading.Tasks;
using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


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
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(int id)
    {
        var country = await _repository.GetByIdAsync(id);
        if (country is null) return NotFound();
        return Ok(new CountryResponse(country.Id, country.Name, country.Abbreviation));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCountryRequest request)
    {
        var country = new Country { Id = request.Id , Name = request.Name, Abbreviation = request.Abbreviation };
        await _repository.Add(country);
        return CreatedAtAction(nameof(GetById), new { id = country.Id }, new CountryResponse(country.Id, country.Name, country.Abbreviation));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(int id, UpdateCountryRequest request)
    {
        var country = await _repository.GetByIdAsync(id);
        if (country is null) return NotFound();

        country.Name = request.Name;
        country.Abbreviation = request.Abbreviation;

        await _repository.UpdateAsync(country);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
