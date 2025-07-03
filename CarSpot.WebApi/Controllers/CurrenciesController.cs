using System;
using System.Threading.Tasks;
using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CarSpot.Application.DTOs;
using System.Collections.Generic;

namespace CarSpot.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrenciesController : ControllerBase
{
    private readonly ICurrencyRepository _repository;

    public CurrenciesController(ICurrencyRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CurrencyResponse>>> GetAll()
    {
        var currencies = await _repository.GetAll();

        var result = currencies.Select(c =>
            new CurrencyResponse(c.Id, c.Name, c.Code, c.Symbol));

        return Ok(result);
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CurrencyResponse>> GetById(Guid id)
    {
        var currency = await _repository.GetById(id);
        if (currency == null) return NotFound();

        return Ok(new CurrencyResponse(currency.Id, currency.Name, currency.Code, currency.Symbol));
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCurrencyRequest request)
    {
        var currency = new Currency
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Code = request.Code,
            Symbol = request.Symbol
        };

        await _repository.Add(currency);

        return Ok(new
        {
            Status = 200,
            Message = "Currency created successfully",
            Data = new
            {
                currency.Name,
                currency.Code,
                currency.Symbol
            }
        });
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCurrencyRequest request)
    {
        var existing = await _repository.GetById(id);
        if (existing == null) return NotFound();

        existing.Name = request.Name;
        existing.Code = request.Code;
        existing.Symbol = request.Symbol;

        await _repository.Update(existing);
        return NoContent();
    }


    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var existing = _repository.GetById(id);
        if (existing == null) return NotFound();

        _repository.Delete(id);
        return NoContent();
    }
}
