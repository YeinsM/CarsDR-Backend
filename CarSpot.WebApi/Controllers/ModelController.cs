using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelsController : ControllerBase
    {
        private readonly IModelRepository _modelRepository;
        private readonly IMakeRepository _makeRepository;
        private readonly IPaginationService _paginationService;

        public ModelsController(IModelRepository modelRepository, IMakeRepository makeRepository, IPaginationService paginationService)
        {
            _modelRepository = modelRepository;
            _makeRepository = makeRepository;
            _paginationService = paginationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameters pagination)
        {
            var query = _modelRepository.Query();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query.Select(m => new ModelDto(
                    m.Id,
                    m.Name!,
                    m.MakeId
                )),
                pagination.PageNumber,
                pagination.PageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var model = await _modelRepository.GetByIdAsync(id);
            if (model == null)
                return NotFound(ApiResponseBuilder.Fail<Model>(404, $"Model with ID {id} not found."));

            return Ok(ApiResponseBuilder.Success(model));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model model)
        {
            var make = await _makeRepository.GetByIdAsync(model.MakeId);
            if (make == null)
                return BadRequest(ApiResponseBuilder.Fail<object>(400, $"Make with ID {model.MakeId} does not exist."));

            await _modelRepository.Add(model);

            return CreatedAtAction(nameof(GetById), new { id = model.Id },
                ApiResponseBuilder.Success(model, "Model created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateModelRequest updated)
        {
            if (id != updated.Id)
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "URL ID does not match body ID."));

            var make = await _makeRepository.GetByIdAsync(updated.MakeId);
            if (make == null)
                return BadRequest(ApiResponseBuilder.Fail<object>(400, $"Make with ID {updated.MakeId} does not exist."));

            var existingModel = await _modelRepository.GetByIdAsync(id);
            if (existingModel == null)
                return NotFound(ApiResponseBuilder.Fail<object>(404, $"Model with ID {id} not found."));

            await _modelRepository.UpdateAsync(updated.Id, updated.Name, updated.MakeId);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await _modelRepository.GetByIdAsync(id);
            if (model == null)
                return NotFound(ApiResponseBuilder.Fail<object>(404, $"Model with ID {id} not found."));

            await _modelRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
