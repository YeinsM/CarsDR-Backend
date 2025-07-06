using System;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.DTOs;
using CarSpot.Application.DTOS;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class ListingsController : ControllerBase
{
    private readonly IListingRepository _repository;

    public ListingsController(IListingRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var listings = await _repository.GetAllAsync();

        var response = listings.Select(listing => new ListingResponse
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

        });

        return Ok(response);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var listing = await _repository.GetByIdAsync(id);
        if (listing == null)
            return NotFound();

        return Ok(listing);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateListingRequest request)
    {
        if (request == null)
            return BadRequest("Request cannot be null");

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

        var savedListing = await _repository.Add(listing);

        return Ok(new
        {
            Message = "Listing created successfully",
            ListingId = savedListing.Id
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateListing(Guid id, [FromBody] UpdateListingRequest request)
    {
        var existingListing = await _repository.GetByIdAsync(id);
        if (existingListing == null)
            return NotFound();


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

        await _repository.UpdateAsync(existingListing);
        return Ok("Listing updated successfully");

    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var listing = await _repository.GetByIdAsync(id);
        if (listing is null) return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
