namespace CarSpot.Application.DTOs;

public record VehicleDto(
    Guid Id,
    string VIN,
    Guid Year,
    string Color,
    Guid ModelId
);