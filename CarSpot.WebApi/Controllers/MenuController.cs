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
    public class MenuController(IMenuRepository repository) : ControllerBase
    {
        private readonly MenuTreeBuilder _menuTreeBuilder = new MenuTreeBuilder();

        [HttpGet]
       [Authorize(Policy = "AdminOrUser")]        public async Task<IActionResult> GetAllAsync()
        {
            System.Collections.Generic.IEnumerable<Menu> menus = await repository.GetAllAsync();
            System.Collections.Generic.List<MenuResponse> tree = _menuTreeBuilder.Build(menus.ToList());
            return Ok(tree);
        }

      
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Menu menu = await repository.GetByIdAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            return Ok(menu);
        }

        
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateMenuRequest request)
        {
            var menu = new Menu(request.Label, request.Icon, request.To);
            await repository.AddAsync(menu);
            return CreatedAtAction(nameof(GetById), new { id = menu.Id }, menu);
        }

        
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateMenuRequest request)
        {
            Menu menu = await repository.GetByIdAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            menu.Update(request.Label, request.Icon, request.To);
            await repository.Update(menu);

            return NoContent();
        }

        
        [HttpDelete("{id}")]
       [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Menu menu = await repository.GetByIdAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
