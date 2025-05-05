namespace CarSpot.Application.DTOs;

public record UpdateVehicleRequest(string VIN, Guid Year, string Color, Guid MakeId, Guid ModelId);