using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Application.DTOs;
using CarSpot.Domain.Entities;
using CarSpot.Application.Services;
using CarSpot.Application.Interfaces;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuRepository _repository;
    private readonly MenuTreeBuilder _menuTreeBuilder;

    public MenuController(IMenuRepository repository)
    {
        _repository = repository;
        _menuTreeBuilder = new MenuTreeBuilder();
    }


    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var menus = await _repository.GetAllAsync();
        var tree = _menuTreeBuilder.Build(menus.ToList());
        return Ok(tree);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var menu = await _repository.GetByIdAsync(id);
        if (menu == null) return NotFound();
        return Ok(menu);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMenuRequest request)
    {
        var menu = new Menu(request.Label, request.Icon, request.To);
        await _repository.AddAsync(menu);
        return CreatedAtAction(nameof(GetById), new { id = menu.Id }, menu);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateMenuRequest request)
    {
        var menu = await _repository.GetByIdAsync(id);
        if (menu == null) return NotFound();

        menu.Update(request.Label, request.Icon, request.To);
        await _repository.Update(menu);

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
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


