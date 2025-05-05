namespace CarSpot.Application.DTOs;

public record UpdateVehicleRequest(string VIN, int Year, string Color, Guid MakeId, Guid ModelId);