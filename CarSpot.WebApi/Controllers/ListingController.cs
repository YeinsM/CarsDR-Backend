using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using CarSpot.Application.DTOs;

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

        var response = listings.Select(l => new
        {
            l.Id,
            l.Title,
            l.Description,
            l.Price,
            l.ListingPrice,
            Currency = new { l.Currency.Id, l.Currency.Name },
            Status = new { l.ListingStatus.Id, l.ListingStatus.Name },
            User = new { l.User.Id, l.User.Email },
            Vehicle = new
            {
                l.Vehicle.Id,
                l.Vehicle.Model.Name,
                Make = l.Vehicle.Model.Make.Name,
                l.Vehicle.Year,
                l.Vehicle.Mileage
            },
            Images = l.Images.Select(img => img.ImageUrl).ToList(),
            l.CreatedAt,
            l.ExpiresAt,
            l.IsFeatured,
            l.FeaturedUntil,
            l.ViewCount
        });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var listing = await _repository.GetByIdAsync(id);
        if (listing is null)
            return NotFound();

        var response = new
        {
            listing.Id,
            listing.Title,
            listing.Description,
            listing.Price,
            listing.ListingPrice,
            Currency = new { listing.Currency.Id, listing.Currency.Name },
            Status = new { listing.ListingStatus.Id, listing.ListingStatus.Name },
            User = new { listing.User.Id, listing.User.Email },
            Vehicle = new
            {
                listing.Vehicle.Id,
                listing.Vehicle.Model.Name,
                Make = listing.Vehicle.Model.Make.Name,
                listing.Vehicle.Year,
                listing.Vehicle.Mileage
            },
            Images = listing.Images.Select(img => img.ImageUrl).ToList(),
            listing.CreatedAt,
            listing.ExpiresAt,
            listing.IsFeatured,
            listing.FeaturedUntil,
            listing.ViewCount
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateListingRequest request)
    {
        var listing = new Listing(
            request.UserId,
            request.VehicleId,
            request.Title,
            request.Description,
            request.Price,
            request.CurrencyId,
            request.ListingStatusId
        );

        await _repository.Add(listing);
        return CreatedAtAction(nameof(GetById), new { id = listing.Id }, listing.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateListingRequest request)
    {
        var listing = await _repository.GetByIdAsync(id);
        if (listing is null) return NotFound();

        listing.VehicleId = request.VehicleId;
        listing.Title = request.Title;
        listing.Description = request.Description;
        listing.Price = request.Price;
        listing.CurrencyId = request.CurrencyId;
        listing.ListingStatusId = request.ListingStatusId;
        listing.IsFeatured = request.IsFeatured;
        listing.FeaturedUntil = request.FeaturedUntil;
        listing.ExpiresAt = request.ExpiresAt;

        await _repository.UpdateAsync(listing);
        return NoContent();
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
