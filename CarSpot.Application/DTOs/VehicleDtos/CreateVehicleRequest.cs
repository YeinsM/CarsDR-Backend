namespace CarSpot.Application.DTOs;

public record CreateVehicleRequest(string VIN, int Year, int ModelId, string Color);
