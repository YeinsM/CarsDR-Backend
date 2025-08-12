using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.DTOS;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ListingsController : ControllerBase
    {
        private readonly IListingRepository _listingRepository;
        private readonly IPaginationService _paginationService;

        public ListingsController(IListingRepository listingRepository, IPaginationService paginationService)
        {
            _listingRepository = listingRepository;
            _paginationService = paginationService;
        }

        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResponse<ListingResponse>>> GetAll([FromQuery] PaginationParameters pagination)
        {
            const int maxPageSize = 100;

            int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
            int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            var query = _listingRepository.Query();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var paginatedResult = await _paginationService.PaginateAsync(
                query.Select(listing => new ListingResponse
                {
                    Id = listing.Id,
                    Title = listing.Title,
                    Description = listing.Description,
                    Price = listing.Price,
                    CurrencyId = listing.CurrencyId,
                    ListingStatusId = listing.ListingStatusId,
                    ExpiresAt = listing.ExpiresAt,
                    IsFeatured = listing.IsFeatured,
                    FeaturedUntil = listing.FeaturedUntil,
                    UserId = listing.UserId,
                    VehicleId = listing.VehicleId
                }),
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }

        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ListingResponse>> GetById(Guid id)
        {
            var listing = await _listingRepository.GetByIdAsync(id);
            if (listing == null)
                return NotFound(new { Message = $"Listing with ID {id} not found." });

            var response = new ListingResponse
            {
                Id = listing.Id,
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                CurrencyId = listing.CurrencyId,
                ListingStatusId = listing.ListingStatusId,
                ExpiresAt = listing.ExpiresAt,
                IsFeatured = listing.IsFeatured,
                FeaturedUntil = listing.FeaturedUntil,
                UserId = listing.UserId,
                VehicleId = listing.VehicleId
            };

            return Ok(response);
        }

       
        [HttpPost]
        [Authorize(Policy = "AdminOrCompany")]
        public async Task<IActionResult> Create([FromBody] CreateListingRequest request)
        {
            if (request == null)
                return BadRequest(new { Message = "Request body cannot be null." });

            var listing = new Listing
            {
                UserId = request.UserId,
                VehicleId = request.VehicleId,
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                ListingPrice = request.ListingPrice,
                CurrencyId = request.CurrencyId,
                ListingStatusId = request.ListingStatusId,
                ExpiresAt = request.ExpiresAt,
                IsFeatured = request.IsFeatured,
                FeaturedUntil = request.FeaturedUntil,
                ViewCount = 0,
            };

            var savedListing = await _listingRepository.Add(listing);

            return CreatedAtAction(nameof(GetById), new { id = savedListing.Id }, new
            {
                Message = "Listing created successfully",
                ListingId = savedListing.Id
            });
        }

       
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOrCompany")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateListingRequest request)
        {
            var existingListing = await _listingRepository.GetByIdAsync(id);
            if (existingListing == null)
                return NotFound(new { Message = $"Listing with ID {id} not found." });

            existingListing.Title = request.Title;
            existingListing.Description = request.Description;
            existingListing.Price = request.Price;
            existingListing.ListingPrice = request.ListingPrice;
            existingListing.CurrencyId = request.CurrencyId;
            existingListing.ListingStatusId = request.ListingStatusId;
            existingListing.ExpiresAt = request.ExpiresAt;
            existingListing.IsFeatured = request.IsFeatured;
            existingListing.FeaturedUntil = request.FeaturedUntil;
            existingListing.UpdatedAt = DateTime.UtcNow;

            await _listingRepository.UpdateAsync(existingListing);

            return Ok(new { Message = "Listing updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOrCompany")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var listing = await _listingRepository.GetByIdAsync(id);
            if (listing == null)
                return NotFound(new { Message = $"Listing with ID {id} not found." });

            await _listingRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
