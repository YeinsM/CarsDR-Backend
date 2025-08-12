using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Application.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Domain.Entities;
using CarSpot.Domain.Common;
using CarSpot.Application.Interfaces.Services;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyRepository _repository;
        private readonly IPaginationService _paginationService;

        public CurrenciesController(ICurrencyRepository repository, IPaginationService paginationService)
        {
            _repository = repository;
            _paginationService = paginationService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<CurrencyResponse>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            var query = _repository.Query();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query.Select(c => new CurrencyResponse(
                    c.Id,
                    c.Name,
                    c.Code,
                    c.Symbol
                )),
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }



        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var currency = await _repository.GetById(id);
            return currency == null
                ? NotFound(ApiResponseBuilder.Fail<CurrencyResponse>(404, $"Currency with ID {id} not found."))
                : Ok(ApiResponseBuilder.Success(new CurrencyResponse(currency.Id, currency.Name, currency.Code, currency.Symbol)));
        }


        [Authorize(Policy = "AdminOnly")]
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


        [Authorize(Policy = "AdminOnly")]
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

        [Authorize(Policy = "AdminOnly")]
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
