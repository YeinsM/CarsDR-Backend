using System;
using System.Threading.Tasks;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            return Ok(comments);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return comment == null ? NotFound() : Ok(comment);
        }


        [HttpGet("vehicle/{vehicleId:guid}")]
        public async Task<IActionResult> GetByVehicleId(Guid vehicleId)
        {
            var comments = await _commentRepository.GetByVehicleIdAsync(vehicleId);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
        {
            var comment = new Comment
            {
                VehicleId = request.VehicleId,
                UserId = request.UserId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepository.CreateAddAsync(comment);
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Comment updated)
        {
            if (id != updated.Id)
                return BadRequest("Comment ID mismatch.");

            var existing = await _commentRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Content = updated.Content;


            await _commentRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _commentRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _commentRepository.DeleteAsync(existing);
            await _commentRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
