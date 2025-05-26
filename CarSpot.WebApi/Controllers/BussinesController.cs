using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BussinesController : ControllerBase
    {
        private readonly IBussinesRepository _bussinesRepository;

        public BussinesController(IBussinesRepository bussinesRepository)
        {
            _bussinesRepository = bussinesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BussinesResponse>>> GetAll()
        {
            var bussinesList = await _bussinesRepository.GetAllAsync();

            var response = bussinesList.Select(b => new BussinesResponse(
                b.Id,
                b.BussinesNumber,
                b.PhoneBussines,
                b.ExtencionBussines,
                b.AddreesBussines
            ));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BussinesResponse>> GetById(Guid id)
        {
            var bussines = await _bussinesRepository.GetByIdAsync(id);

            if (bussines == null) return NotFound();

            var response = new BussinesResponse(
                bussines.Id,
                bussines.BussinesNumber,
                bussines.PhoneBussines,
                bussines.ExtencionBussines,
                bussines.AddreesBussines
            );

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateBussinesRequest request)
        {
            var bussines = new Bussines
            {
                Id = Guid.NewGuid(),
                BussinesNumber = request.BussinesNumber,
                PhoneBussines = request.PhoneBussines,
                ExtencionBussines = request.ExtencionBussines,
                AddreesBussines = request.AddreesBussines
            };

            await _bussinesRepository.AddAsync(bussines);
            await _bussinesRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = bussines.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateBussinesRequest request)
        {
            if (id != request.Id) return BadRequest();

            var bussines = await _bussinesRepository.GetByIdAsync(id);
            if (bussines == null) return NotFound();

            bussines.BussinesNumber = request.BussinesNumber;
            bussines.PhoneBussines = request.PhoneBussines;
            bussines.ExtencionBussines = request.ExtencionBussines;
            bussines.AddreesBussines = request.AddreesBussines;

            _bussinesRepository.Update(bussines);
            await _bussinesRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var bussines = await _bussinesRepository.GetByIdAsync(id);
            if (bussines == null) return NotFound();

            _bussinesRepository.Delete(bussines);
            await _bussinesRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
