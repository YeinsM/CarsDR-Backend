using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.DTOs.MakeDtos;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Entities;
using CarSpot.Application.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Domain.Common;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MakesController : ControllerBase
    {
        private readonly IMakeRepository _repository;
        private readonly IPaginationService _paginationService;

        public MakesController(IMakeRepository repository, IPaginationService paginationService)
        {
            _repository = repository;
            _paginationService = paginationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<MakeDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            var query = _repository.Query();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query.Select(m => new MakeDto(m.Id, m.Name!)),
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }

        [HttpGet("{id}")]
         [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item is null
                ? NotFound(ApiResponseBuilder.Fail<MakeDto>(404, "Make not found"))
                : Ok(ApiResponseBuilder.Success(new MakeDto(item.Id, item.Name)));
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create([FromBody] CreateMakeRequest request)
        {
            var make = new Make(request.Name);
            await _repository.Add(make);

            var dto = new MakeDto(make.Id, make.Name);

            return Ok(ApiResponseBuilder.Success(dto, "Make created successfully"));
        }

        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update([FromBody] UpdateMakeRequest request)
        {
            var existingMake = await _repository.GetByIdAsync(request.Id);
            if (existingMake == null)
                return NotFound(ApiResponseBuilder.Fail<MakeDto>(404, "Make not found"));

            await _repository.UpdateAsync(request.Id, request.Name);

            return Ok(ApiResponseBuilder.Success(200, "Make updated successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var make = await _repository.GetByIdAsync(id);
            if (make is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Make not found"));

            await _repository.RemoveAsync(id);

            return Ok(ApiResponseBuilder.Success("Make deleted successfully"));
        }
    }
}
