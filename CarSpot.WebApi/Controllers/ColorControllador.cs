using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class ColorController : ControllerBase
{
    private readonly IColorRepository _colorRepository;

    public ColorController(IColorRepository colorRepository)
    {
        _colorRepository = colorRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var colors = await _colorRepository.GetAllAsync();
        var response = colors.Select(c => new ColorResponse
        {
            Id = c.Id,
            Name = c.Name
        });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var color = await _colorRepository.GetByIdAsync(id);
        if (color is null) return NotFound();

        return Ok(new ColorResponse
        {
            Id = color.Id,
            Name = color.Name
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateColorRequest request)
    {
        var color = new Color(request.Name);
        await _colorRepository.AddAsync(color);

        return CreatedAtAction(nameof(GetById), new { id = color.Id }, color.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateColorRequest request)
    {
        var color = await _colorRepository.GetByIdAsync(id);
        if (color is null) return NotFound();

        typeof(Color).GetProperty("Name")?.SetValue(color, request.Name);
        await _colorRepository.UpdateAsync(color);

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var color = await _colorRepository.GetByIdAsync(id);
        if (color is null) return NotFound();

        await _colorRepository.DeleteAsync(color);
        return NoContent();
    }
}
