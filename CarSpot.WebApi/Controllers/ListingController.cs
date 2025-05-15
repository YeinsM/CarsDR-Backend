using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using System.Linq;








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
        var Listings = await _repository.GetAllAsync();
        var response = Listings.Select(l => new ListingResponse
        {
            Id = l.Id,
            Price = l.Price,
            Currency = l.User.select()
           Images = l.Images.Select(img => img.ImageUrl).ToList(),
            CreatedAt = l.CreatedAt
        });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var Listing = await _repository.GetByIdAsync(id);
        if (Listing is null) return NotFound();

        var response = new ListingResponse
        {
            Id = Listing.Id,
            Make = Listing.Make.Name,
            Model = Listing.Model.Name,
            Color = Listing.Color.Name,
            Price = Listing.Price,
            Currency = Listing.Currency,
            Place = Listing.Place,
            Version = Listing.Version,
            Images = Listing.Images,
            CreatedAt = Listing.CreatedAt
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateListingRequest request)
    {
        var Listing = new Listing(
            request.UserId,
            request.MakeId,
            request.ModelId,
            request.ColorId,
            request.Price,
            request.Currency,
            request.Place,
            request.Version,
            request.Images
        );

        await _repository.Add(Listing);
        return CreatedAtAction(nameof(GetById), new { id = Listing.Id }, Listing.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CreateListingRequest request)
    {
        var Listing = await _repository.GetByIdAsync(id);
        if (Listing is null) return NotFound();

      


        Listing = new Listing(
            request.UserId,
            request.MakeId,
            request.ModelId,
            request.ColorId,
            request.Price,
            request.Currency,
            request.Place,
            request.Version,
            request.Images
        );

        typeof(Listing).GetProperty("Id")!.SetValue(Listing, id);
        await _repository.UpdateAsync(Listing);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var Listing = await _repository.GetByIdAsync(id);
        if (Listing is null) return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
