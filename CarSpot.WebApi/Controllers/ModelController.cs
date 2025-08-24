using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using CarSpot.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelsController(IModelRepository modelRepository, IMakeRepository makeRepository, IPaginationService paginationService) : PaginatedControllerBase(paginationService)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<ModelDto>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            IQueryable<ModelDto> query = modelRepository.Query()
                .Select(m => new ModelDto(
                    m.Id,
                    m.Name!,
                    m.MakeId
                ));

            return await GetPaginatedResultAsync(query, pagination);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Model? model = await modelRepository.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound(ApiResponseBuilder.Fail<Model>(404, $"Model with ID {id} not found."));
            }

            return Ok(ApiResponseBuilder.Success(model));
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create([FromBody] Model model)
        {
            Make make = await makeRepository.GetByIdAsync(model.MakeId);
            if (make == null)
            {
                return BadRequest(ApiResponseBuilder.Fail<object>(400, $"Make with ID {model.MakeId} does not exist."));
            }

            await modelRepository.Add(model);

            return CreatedAtAction(nameof(GetById), new { id = model.Id },
                ApiResponseBuilder.Success(model, "Model created successfully."));
        }

        [HttpPut("{id}")]
         [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateModelRequest updated)
        {
            if (id != updated.Id)
            {
                return BadRequest(ApiResponseBuilder.Fail<object>(400, "URL ID does not match body ID."));
            }

            Make make = await makeRepository.GetByIdAsync(updated.MakeId);
            if (make == null)
            {
                return BadRequest(ApiResponseBuilder.Fail<object>(400, $"Make with ID {updated.MakeId} does not exist."));
            }

            Model? existingModel = await modelRepository.GetByIdAsync(id);
            if (existingModel == null)
            {
                return NotFound(ApiResponseBuilder.Fail<object>(404, $"Model with ID {id} not found."));
            }

            await modelRepository.UpdateAsync(updated.Id, updated.Name, updated.MakeId);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Model? model = await modelRepository.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound(ApiResponseBuilder.Fail<object>(404, $"Model with ID {id} not found."));
            }

            await modelRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
