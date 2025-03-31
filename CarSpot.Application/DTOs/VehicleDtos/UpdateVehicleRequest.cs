namespace CarSpot.Application.DTOs;

public record UpdateVehicleRequest(string VIN, int Year, string Color, int MakeId, int ModelId);