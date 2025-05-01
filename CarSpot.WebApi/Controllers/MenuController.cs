using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.DTOs;
using CarSpot.Domain.Entities;
using CarSpot.Application.Services;
using CarSpot.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuRepository _repository;

    public MenuController(IMenuRepository repository)
    {
        _repository = repository;
    }

   
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var menus = await _repository.GetAllAsync();
        return Ok(menus);
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var menu = await _repository.GetByIdAsync(id);
        if (menu == null) return NotFound();
        return Ok(menu);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMenuRequest request)
    {
        var menu = new Menu(request.Label, request.Icon, request.Menub, request.To);
        await _repository.AddAsync(menu);
        return CreatedAtAction(nameof(GetById), new { id = menu.Id }, menu);
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateMenuRequest request)
    {
        var menu = await _repository.GetByIdAsync(id);
        if (menu == null) return NotFound();

        menu.Update(request.Label, request.Icon, request.Menub, request.To);
        _repository.Update(menu);

        return NoContent();
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var menu = await _repository.GetByIdAsync(id);
        if (menu == null)
        {
            return NotFound(); 
        }

        await _repository.DeleteAsync(id);  
        return NoContent(); 
    }
}


