using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.DTOs.BusinessDtos;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using CarSpot.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class BusinessController(IBusinessRepository businessRepository, IPaginationService paginationService) : PaginatedControllerBase(paginationService)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<BusinessResponse>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            IQueryable<BusinessResponse> query = businessRepository.Query()
                .Select(b => new BusinessResponse(
                    b.Id,
                    b.Name!,
                    b.BusinessNumber!,
                    b.Phone,
                    b.Extension,
                    b.Address
                ));

            return await GetPaginatedResultAsync(query, pagination);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<ActionResult<BusinessResponse>> GetById(Guid id)
        {
            Business? business = await businessRepository.GetByIdAsync(id);

            if (business == null)
            {
                return NotFound();
            }

            var response = new BusinessResponse(
                business.Id,
                business.Name!,
                business.BusinessNumber!,
                business.Phone,
                business.Extension,
                business.Address
            );

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
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

                await businessRepository.Add(business);
                await businessRepository.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = business.Id }, new
                {
                    message = "Business created successfully!",
                    id = business.Id
                });
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
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> Update(Guid id, UpdateBusinessRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            Business? bussines = await businessRepository.GetByIdAsync(id);
            if (bussines == null)
            {
                return NotFound();
            }

            bussines.BusinessNumber = request.BusinessNumber;
            bussines.Phone = request.Phone;
            bussines.Extension = request.Extension;
            bussines.Address = request.Address;

            businessRepository.Update(bussines);
            await businessRepository.SaveChangesAsync();

            return Ok(new { message = "Business updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> Delete(Guid id)
        {
            Business? bussines = await businessRepository.GetByIdAsync(id);
            if (bussines == null)
            {
                return NotFound();
            }

            businessRepository.Delete(bussines);
            await businessRepository.SaveChangesAsync();

            return Ok(new { message = "Business deleted successfully." });
        }
    }
}
