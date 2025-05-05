namespace CarSpot.Application.DTOs;

public record CreateVehicleRequest(string VIN, int Year, Guid ModelId, string Color);
