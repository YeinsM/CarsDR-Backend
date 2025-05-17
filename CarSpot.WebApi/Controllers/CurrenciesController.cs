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
    public ActionResult<IEnumerable<CurrencyResponse>> GetAll()
    {
        var currencies = _repository.GetAll()
            .Select(c => new CurrencyResponse(c.Id, c.Name, c.Code, c.Symbol));
        return Ok(currencies);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<CurrencyResponse> GetById(Guid id)
    {
        var currency = _repository.GetById(id);
        if (currency == null) return NotFound();

        return Ok(new CurrencyResponse(currency.Id, currency.Name, currency.Code, currency.Symbol));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateCurrencyRequest request)
    {
        var currency = new Currency(request.Name, request.Code, request.Symbol);
        _repository.Add(currency);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] UpdateCurrencyRequest request)
    {
        var existing = _repository.GetById(id);
        if (existing == null) return NotFound();

        existing.Name = request.Name;
        existing.Code = request.Code;
        existing.Symbol = request.Symbol;

        _repository.Update(existing);
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
