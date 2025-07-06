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
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessRepository _businessRepository;

        public BusinessController(IBusinessRepository businessRepository)
        {
            _businessRepository = businessRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessResponse>>> GetAll()
        {
            var bussinesList = await _businessRepository.GetAllAsync();

            var response = bussinesList.Select(b => new BusinessResponse(
                b.Id,
                b.Name,
                b.BusinessNumber,
                b.Phone,
                b.Extension,
                b.Address
            ));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessResponse>> GetById(Guid id)
        {
            var business = await _businessRepository.GetByIdAsync(id);

            if (business == null) return NotFound();

            var response = new BusinessResponse(
                business.Id,
                business.Name,
                business.BusinessNumber,
                business.Phone,
                business.Extension,
                business.Address
            );

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateBusinessRequest request)
        {
            try
            {
                var business = new Business
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    BusinessNumber = request.BusinessNumber,
                    Phone = request.Phone,
                    Extension = request.Extension,
                    Address = request.Address,
                    UpdatedAt = DateTime.UtcNow
                };

                await _businessRepository.Add(business);
                await _businessRepository.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = business.Id }, null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateBusinessRequest request)
        {
            if (id != request.Id) return BadRequest();

            var bussines = await _businessRepository.GetByIdAsync(id);
            if (bussines == null) return NotFound();

            bussines.BusinessNumber = request.BusinessNumber;
            bussines.Phone = request.Phone;
            bussines.Extension = request.Extension;
            bussines.Address = request.Address;

            _businessRepository.Update(bussines);
            await _businessRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var bussines = await _businessRepository.GetByIdAsync(id);
            if (bussines == null) return NotFound();

            _businessRepository.Delete(bussines);
            await _businessRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
