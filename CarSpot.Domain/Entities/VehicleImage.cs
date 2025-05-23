using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;

public class VehicleImage
{
public Guid Id { get; set; }
public int VehicleId { get; set; }
public string? ImageUrl { get; set; }
public Vehicle? Vehicle { get; set; }
public Guid ListingId { get; set; }
public Listing Listing { get; set; }=null!;


}