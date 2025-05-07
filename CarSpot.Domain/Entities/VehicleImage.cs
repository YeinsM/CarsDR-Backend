using CarSpot.Domain.Entities;

public class VehicleImage
{
public Guid Id { get; set; }
public Guid VehicleId { get; set; }
public string ImageUrl { get; set; }
public Vehicle Vehicle { get; set; }
}