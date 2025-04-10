using Microsoft.AspNetCore.Mvc;
using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using CarSpot.Application.DTOs;

[ApiController]
[Route("api/[controller]")]
public class MakeController : ControllerBase
{
    private readonly IMakeRepository _makeRepository;

    public MakeController(IMakeRepository makeRepository)
    {
        _makeRepository = makeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Make>>> GetAll()
    {
        var makes = await _makeRepository.GetAllAsync();
        return Ok(makes);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Make>> GetById(int id)
    {
        var make = await _makeRepository.GetByIdAsync(id);
        if (make == null) return NotFound();

        return Ok(make);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMakeRequest request)
    {
        var make = new Make(request.Name);
        await _makeRepository.AddAsync(make);
        return CreatedAtAction(nameof(GetById), new { id = make.Id }, make);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMakeRequest updateRequest)
    {
        var make = await _makeRepository.GetByIdAsync(id);
        if (make is null)
        {
            return NotFound();
        }

        
        make.Update(updateRequest.Name);

        await _makeRepository.UpdateAsync(make);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var make = await _makeRepository.GetByIdAsync(id);
        if (make == null) return NotFound();

        await _makeRepository.DeleteAsync(make);
        return NoContent();
    }
}
