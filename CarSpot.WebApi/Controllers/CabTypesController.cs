using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.Common.Responses;


namespace CarSpot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CabTypesController : ControllerBase
    {
        private readonly IAuxiliarRepository<CabType> _repository;

        public CabTypesController(IAuxiliarRepository<CabType> repository)
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

            if (item is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Item not found."));

            return Ok(ApiResponseBuilder.Success(item, "Item found successfully."));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CabType cabType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseBuilder.Fail<string>(400, "Invalid input."));

            await _repository.Add(cabType);

            var response = ApiResponseBuilder.Success(cabType, "CabType created successfully.");

            return CreatedAtAction(nameof(GetById), new { id = cabType.Id }, response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CabType updated)
        {
            if (id != updated.Id)
                return BadRequest(ApiResponseBuilder.Fail<string>(400, "The ID in the URL does not match the request body."));

            var existing = await _repository.GetByIdAsync(id);

            if (existing is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Cab type not found."));

            existing.Name = updated.Name;
            await _repository.UpdateAsync(existing);

            return Ok(ApiResponseBuilder.Success<string>(null, "Cab type updated successfully."));
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cabType = await _repository.GetByIdAsync(id);

            if (cabType is null)
                return NotFound(ApiResponseBuilder.Fail<string>(404, "Cab type not found."));

            await _repository.DeleteAsync(cabType);

            return Ok(ApiResponseBuilder.Success<string>(null, "Cab type deleted successfully."));
        }

    }
}
