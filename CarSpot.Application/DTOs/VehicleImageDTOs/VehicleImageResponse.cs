namespace CarSpot.Application.DTOs;

public record VehicleImageResponse(
    Guid Id,
    Guid VehicleId,
    string ImageUrl
);
