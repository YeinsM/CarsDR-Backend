using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
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
    public class CurrenciesController(ICurrencyRepository repository, IPaginationService paginationService) : PaginatedControllerBase(paginationService)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<CurrencyResponse>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            IQueryable<CurrencyResponse> query = repository.Query()
                .Select(c => new CurrencyResponse(
                    c.Id,
                    c.Name,
                    c.Code,
                    c.Symbol
                ));

            return await GetPaginatedResultAsync(query, pagination);
        }



        
        [HttpGet("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Currency? currency = await repository.GetById(id);
            return currency == null
                ? NotFound(ApiResponseBuilder.Fail<CurrencyResponse>(404, $"Currency with ID {id} not found."))
                : Ok(ApiResponseBuilder.Success(new CurrencyResponse(currency.Id, currency.Name, currency.Code, currency.Symbol)));
        }


        
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateCurrencyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Code) || string.IsNullOrWhiteSpace(request.Symbol))
            {
                return BadRequest(ApiResponseBuilder.Fail<CurrencyResponse>(400, "Invalid currency data. Name, code, and symbol are required."));
            }

            var currency = new Currency
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                Symbol = request.Symbol
            };

            await repository.Add(currency);

            var response = new CurrencyResponse(currency.Id, currency.Name, currency.Code, currency.Symbol);
            return CreatedAtAction(nameof(GetById), new { id = currency.Id }, ApiResponseBuilder.Success(response, "Currency created successfully."));
        }


        
        [HttpPut("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCurrencyRequest request)
        {
            Currency? existing = await repository.GetById(id);
            if (existing == null)
            {
                return NotFound(ApiResponseBuilder.Fail<CurrencyResponse>(404, $"Currency with ID {id} not found."));
            }

            existing.Name = request.Name;
            existing.Code = request.Code;
            existing.Symbol = request.Symbol;

            await repository.Update(existing);
            return Ok(ApiResponseBuilder.Success(new CurrencyResponse(existing.Id, existing.Name, existing.Code, existing.Symbol), "Currency updated successfully."));
        }

        
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Currency? existing = await repository.GetById(id);
            if (existing == null)
            {
                return NotFound(ApiResponseBuilder.Fail<CurrencyResponse>(404, $"Currency with ID {id} not found."));
            }

            await repository.Delete(id);
            return Ok(ApiResponseBuilder.Success<CurrencyResponse>(null, "Currency deleted successfully."));
        }
    }
}
