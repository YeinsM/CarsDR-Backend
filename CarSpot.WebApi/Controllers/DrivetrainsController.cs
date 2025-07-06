using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrivetrainsController : ControllerBase
    {
        private readonly IAuxiliarRepository<Drivetrain> _repository;

        public DrivetrainsController(IAuxiliarRepository<Drivetrain> repository)
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
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Drivetrain drivetrain)
        {
            await _repository.Add(drivetrain);
            return CreatedAtAction(nameof(GetById), new { id = drivetrain.Id }, drivetrain);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Drivetrain updated)
        {
            if (id != updated.Id) return BadRequest();
            await _repository.UpdateAsync(updated);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
