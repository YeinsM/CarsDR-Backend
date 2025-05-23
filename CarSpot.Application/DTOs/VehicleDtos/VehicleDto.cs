namespace CarSpot.Application.DTOs;

public record VehicleDto(
    Guid Id,
    string VIN,
    int Year,
    string Color,
    int ModelId
);