using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;

public class VehicleImage : BaseAuxiliar
{

public Guid VehicleId { get; set; }
public string? ImageUrl { get; set; }
public Vehicle? Vehicle { get; set; }

}