using System;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelsController : ControllerBase
    {
        private readonly IModelRepository _repository;
        private readonly IMakeRepository _makeRepository;

        public ModelsController(IModelRepository repository, IMakeRepository makeRepository)
        {
            _repository = repository;
            _makeRepository = makeRepository;
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
        public async Task<IActionResult> Create([FromBody] Model model)
        {
            var make = await _makeRepository.GetByIdAsync(model.MakeId);
            if (make is null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ApiResponseBuilder.Fail<object?>(StatusCodes.Status400BadRequest, $"Make with ID {model.MakeId} does not exist."));
            }

            await _repository.Add(model);

            return StatusCode(StatusCodes.Status201Created,
                ApiResponseBuilder.Success(model, "Model created successfully."));
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateModelRequest updated)
        {
            if (id != updated.Id)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ApiResponseBuilder.Fail<object?>(StatusCodes.Status400BadRequest, "URL ID does not match body ID."));
            }

            var make = await _makeRepository.GetByIdAsync(updated.MakeId);
            if (make is null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ApiResponseBuilder.Fail<object?>(StatusCodes.Status400BadRequest, $"Make with ID {updated.MakeId} does not exist."));
            }

            await _repository.UpdateAsync(updated.Id, updated.Name, updated.MakeId);

            return StatusCode(StatusCodes.Status204NoContent,
                ApiResponseBuilder.Success<object?>(null, "Model updated successfully."));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await _repository.GetByIdAsync(id);
            if (model is null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    ApiResponseBuilder.Fail<object?>(StatusCodes.Status404NotFound, $"Model with ID {id} does not exist."));
            }

            await _repository.DeleteAsync(id);

            return StatusCode(StatusCodes.Status204NoContent,
                ApiResponseBuilder.Success<object?>(null, "Model deleted successfully."));
        }

    }
}
