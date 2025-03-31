namespace CarSpot.Application.DTOs;

public record CreateVehicleRequest(string VIN, int Year, string Color, int MakeId, int ModelId);
