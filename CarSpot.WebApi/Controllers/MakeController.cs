using System;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.DTOs.MakeDtos;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.Common.Responses;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MakesController : ControllerBase
    {
        private readonly IMakeRepository _repository;

        public MakesController(IMakeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMakeRequest request)
        {
            var make = new Make(request.Name);
            await _repository.Add(make);

            var dto = new MakeDto(make.Id, make.Name);

            return Ok(ApiResponseBuilder.Success(dto, "Make created successfully"));
        }




        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMakeRequest request)
        {
            var existingMake = await _repository.GetByIdAsync(request.Id);
            if (existingMake == null)
            {
                return NotFound(ApiResponseBuilder.Fail<MakeDto>(404, "Make not found"));
            }

            await _repository.UpdateAsync(request.Id, request.Name);

            return Ok(ApiResponseBuilder.Success(200, "Make updated successfully"));
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var make = await _repository.GetByIdAsync(id);
            if (make is null)
            {
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Make not found"));
            }

            await _repository.RemoveAsync(id);

            return Ok(ApiResponseBuilder.Success("Make deleted successfully"));
        }



    }
}
