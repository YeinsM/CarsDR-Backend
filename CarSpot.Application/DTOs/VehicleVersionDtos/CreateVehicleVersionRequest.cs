namespace CarSpot.Application.DTOs;

public record CreateVehicleVersionRequest(string Name, Guid ModelId);