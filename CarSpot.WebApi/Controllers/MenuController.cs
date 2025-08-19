using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Application.Services;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.API.Controllers
{
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
       [Authorize(Policy = "AdminOrUser")]        public async Task<IActionResult> GetAllAsync()
        {
            var menus = await _repository.GetAllAsync();
            var tree = _menuTreeBuilder.Build(menus.ToList());
            return Ok(tree);
        }

      
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var menu = await _repository.GetByIdAsync(id);
            if (menu == null)
                return NotFound();

            return Ok(menu);
        }

        
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateMenuRequest request)
        {
            var menu = new Menu(request.Label, request.Icon, request.To);
            await _repository.AddAsync(menu);
            return CreatedAtAction(nameof(GetById), new { id = menu.Id }, menu);
        }

        
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateMenuRequest request)
        {
            var menu = await _repository.GetByIdAsync(id);
            if (menu == null)
                return NotFound();

            menu.Update(request.Label, request.Icon, request.To);
            await _repository.Update(menu);

            return NoContent();
        }

        
        [HttpDelete("{id}")]
       [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var menu = await _repository.GetByIdAsync(id);
            if (menu == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
