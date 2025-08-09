using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Domain.Entities;

namespace CarSpot.WebApi.Controllers
{
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
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            const int maxPageSize = 100;

            if (pageNumber <= 0)
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "Page number must be greater than zero."));

            if (pageSize <= 0)
                pageSize = 1;
            else if (pageSize > maxPageSize)
                pageSize = maxPageSize;

            var (items, totalItems) = await _repository.GetPagedAsync(pageNumber, pageSize);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var responseItems = items.Select(c => new CurrencyResponse(c.Id, c.Name, c.Code, c.Symbol));

            var paginatedResponse = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = responseItems
            };

            return Ok(ApiResponseBuilder.Success(paginatedResponse, "List of currencies retrieved successfully."));
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var currency = await _repository.GetById(id);
            return currency == null
                ? NotFound(ApiResponseBuilder.Fail<CurrencyResponse>(404, $"Currency with ID {id} not found."))
                : Ok(ApiResponseBuilder.Success(new CurrencyResponse(currency.Id, currency.Name, currency.Code, currency.Symbol)));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCurrencyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Code) || string.IsNullOrWhiteSpace(request.Symbol))
                return BadRequest(ApiResponseBuilder.Fail<CurrencyResponse>(400, "Invalid currency data. Name, code, and symbol are required."));

            var currency = new Currency
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                Symbol = request.Symbol
            };

            await _repository.Add(currency);

            var response = new CurrencyResponse(currency.Id, currency.Name, currency.Code, currency.Symbol);
            return CreatedAtAction(nameof(GetById), new { id = currency.Id }, ApiResponseBuilder.Success(response, "Currency created successfully."));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCurrencyRequest request)
        {
            var existing = await _repository.GetById(id);
            if (existing == null)
                return NotFound(ApiResponseBuilder.Fail<CurrencyResponse>(404, $"Currency with ID {id} not found."));

            existing.Name = request.Name;
            existing.Code = request.Code;
            existing.Symbol = request.Symbol;

            await _repository.Update(existing);
            return Ok(ApiResponseBuilder.Success(new CurrencyResponse(existing.Id, existing.Name, existing.Code, existing.Symbol), "Currency updated successfully."));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _repository.GetById(id);
            if (existing == null)
                return NotFound(ApiResponseBuilder.Fail<CurrencyResponse>(404, $"Currency with ID {id} not found."));

            await _repository.Delete(id);
            return Ok(ApiResponseBuilder.Success<CurrencyResponse>(null, "Currency deleted successfully."));
        }
    }
}
