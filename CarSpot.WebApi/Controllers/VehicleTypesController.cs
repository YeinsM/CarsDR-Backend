using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence;
using CarSpot.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;


namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleTypesController : ControllerBase
    {
        private readonly IAuxiliarRepository<VehicleType> _repository;

        public VehicleTypesController(IAuxiliarRepository<VehicleType> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAllAsync();
            var mapped = result.Select(x => new VehicleTypeDto(x.Id, x.Name));
            return Ok(mapped);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleTypeRequest request)
        {
            var entity = new VehicleType { Name = request.Name };
            await _repository.Add(entity);
            return Ok(new VehicleTypeDto(entity.Id, entity.Name));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVehicleTypeRequest request)
        {
            var entity = new VehicleType { Name = request.Name };
            var updated = await _repository.UpdateAsync(entity);
            return updated is null
                ? NotFound()
                : Ok(new VehicleTypeDto(updated.Id, updated.Name));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tipo = await _repository.GetByIdAsync(id);
            return tipo is null ? NotFound() : Ok(tipo);
        }
    }
}
