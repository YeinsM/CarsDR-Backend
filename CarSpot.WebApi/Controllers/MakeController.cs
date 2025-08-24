using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.DTOs;
using CarSpot.Application.DTOs.MakeDtos;
using CarSpot.Application.Interfaces.Repositories;
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
    public class MakesController(IMakeRepository repository, IPaginationService paginationService) : PaginatedControllerBase(paginationService)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<MakeDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            IQueryable<MakeDto> query = repository.Query()
                .Select(m => new MakeDto(m.Id, m.Name!));

            return await GetPaginatedResultAsync(query, pagination);
        }

        [HttpGet("{id}")]
         [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Make? item = await repository.GetByIdAsync(id);
            return item is null
                ? NotFound(ApiResponseBuilder.Fail<MakeDto>(404, "Make not found"))
                : Ok(ApiResponseBuilder.Success(new MakeDto(item.Id, item.Name)));
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create([FromBody] CreateMakeRequest request)
        {
            var make = new Make(request.Name);
            await repository.Add(make);

            var dto = new MakeDto(make.Id, make.Name);

            return Ok(ApiResponseBuilder.Success(dto, "Make created successfully"));
        }

        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update([FromBody] UpdateMakeRequest request)
        {
            Make existingMake = await repository.GetByIdAsync(request.Id);
            if (existingMake == null)
            {
                return NotFound(ApiResponseBuilder.Fail<MakeDto>(404, "Make not found"));
            }

            await repository.UpdateAsync(request.Id, request.Name);

            return Ok(ApiResponseBuilder.Success(200, "Make updated successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Make? make = await repository.GetByIdAsync(id);
            if (make is null)
            {
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Make not found"));
            }

            await repository.RemoveAsync(id);

            return Ok(ApiResponseBuilder.Success("Make deleted successfully"));
        }
    }
}
