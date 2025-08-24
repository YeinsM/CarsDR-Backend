using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleTypesController(IAuxiliarRepository<VehicleType> repository) : ControllerBase
    {
        [HttpGet]
       [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            System.Collections.Generic.IEnumerable<VehicleType> result = await repository.GetAllAsync();
            System.Collections.Generic.IEnumerable<VehicleTypeDto> mapped = result.Select(x => new VehicleTypeDto(x.Id, x.Name));
            return Ok(mapped);
        }

      
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> GetById(int id)
        {
            VehicleType? tipo = await repository.GetByIdAsync(id);
            return tipo is null ? NotFound() : Ok(new VehicleTypeDto(tipo.Id, tipo.Name));
        }

    
        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create([FromBody] CreateVehicleTypeRequest request)
        {
            var entity = new VehicleType { Name = request.Name };
            await repository.Add(entity);
            return Ok(new VehicleTypeDto(entity.Id, entity.Name));
        }

        
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVehicleTypeRequest request)
        {
            VehicleType? existing = await repository.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound();
            }

            existing.Name = request.Name;
            VehicleType? updated = await repository.UpdateAsync(existing);

            return updated is null
                ? NotFound()
                : Ok(new VehicleTypeDto(updated.Id, updated.Name));
        }
    }
}
