using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CarSpot.Application.DTOS;
using CarSpot.Domain.Common;

namespace CarSpot.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ListingsController : ControllerBase
    {
        private readonly IListingRepository _listingRepository;
        private readonly IPaginationService _paginationService;
        private readonly IPlanService _planService;

        public ListingsController(IListingRepository listingRepository, IPaginationService paginationService, IPlanService planService)
        {
            _listingRepository = listingRepository;
            _paginationService = paginationService;
            _planService = planService;
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
                    VehicleId = listing.VehicleId,
                    IsHighlighted = listing.IsHighlighted,
                    HighlightFrom = listing.HighlightFrom,
                    HighlightUntil = listing.HighlightUntil
                }),
                pageNumber,
                pageSize,
                baseUrl
            );

            return Ok(paginatedResult);
        }

        
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOrUser")]
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
                VehicleId = listing.VehicleId,
                IsHighlighted = listing.IsHighlighted,
                HighlightFrom = listing.HighlightFrom,
                HighlightUntil = listing.HighlightUntil
            };

            listing.ViewCount++;
            await _listingRepository.UpdateAsync(listing);

            return Ok(response);
        }

        
        [HttpPost]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Create([FromBody] CreateListingRequest request)
        {
            if (request == null)
                return BadRequest(new { Message = "Request body cannot be null." });

            var listing = new Listing(
                request.UserId,
                request.VehicleId,
                request.Title,
                request.Description,
                request.Price,
                request.CurrencyId,
                request.ListingStatusId
            )
            {
                ListingPrice = request.ListingPrice,
                ExpiresAt = request.ExpiresAt
            };

            var savedListing = await _listingRepository.Add(listing);

            return CreatedAtAction(nameof(GetById), new { id = savedListing.Id }, new
            {
                Message = "Listing created successfully",
                ListingId = savedListing.Id
            });
        }

        
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOrUser")]
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
            existingListing.UpdatedAt = DateTime.UtcNow;

            await _listingRepository.UpdateAsync(existingListing);

            return Ok(new { Message = "Listing updated successfully" });
        }

        
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var listing = await _listingRepository.GetByIdAsync(id);
            if (listing == null)
                return NotFound(new { Message = $"Listing with ID {id} not found." });

            await _listingRepository.DeleteAsync(id);

            return NoContent();
        }

        
        [Authorize(Roles = "Seller,Admin")]
        [HttpPost("{listingId}/mark-feature")]
        public async Task<IActionResult> MarkAsFeatured(Guid listingId, [FromBody] FeatureListingRequest request)
        {
            var listing = await _listingRepository.GetByIdAsync(listingId);
            if (listing == null)
                return NotFound(new { message = "Listing not found." });

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Invalid token or user ID not found." });

            var userId = Guid.Parse(userIdClaim);

            var hasPlan = await _planService.UserHasActivePlan(userId);
            if (!hasPlan)
                return BadRequest(new { message = "You need an active plan to feature a listing." });

            listing.MarkAsFeatured(request.StartDate, request.EndDate);
            await _listingRepository.UpdateAsync(listing);

            return Ok(new
            {
                message = "Listing featured successfully.",
                listingId = listing.Id,
                featuredFrom = listing.FeaturedFrom,
                featuredUntil = listing.FeaturedUntil
            });
        }

        [HttpPost("{listingId}/remove-featured")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> RemoveFeatured(Guid listingId)
        {
            var listing = await _listingRepository.GetByIdAsync(listingId);
            if (listing == null)
                return NotFound(new { message = "Listing not found." });

            listing.RemoveFeatured();
            await _listingRepository.UpdateAsync(listing);

            return Ok(new { message = "Listing removed from featured successfully.", listingId = listing.Id });
        }

        
        [HttpPost("{listingId}/highlight")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HighlightListing(Guid listingId, [FromBody] HighlightListingRequest request)
        {
            var listing = await _listingRepository.GetByIdAsync(listingId);
            if (listing == null)
                return NotFound(new { message = "Listing not found." });

            listing.MarkAsHighlighted(request.StartDate, request.EndDate);
            await _listingRepository.UpdateAsync(listing);

            return Ok(new
            {
                message = "Listing highlighted successfully.",
                listingId = listing.Id,
                highlightFrom = listing.HighlightFrom,
                highlightUntil = listing.HighlightUntil
            });
        }

        [HttpPost("{listingId}/remove-highlight")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveHighlight(Guid listingId)
        {
            var listing = await _listingRepository.GetByIdAsync(listingId);
            if (listing == null)
                return NotFound(new { message = "Listing not found." });

            listing.RemoveHighlighted();
            await _listingRepository.UpdateAsync(listing);

            return Ok(new { message = "Listing removed from highlight successfully.", listingId = listing.Id });
        }
    }
}
