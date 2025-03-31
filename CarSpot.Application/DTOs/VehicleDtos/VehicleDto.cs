namespace CarSpot.Application.DTOs;

public record VehicleDto(
    int Id,
    string VIN,
    int Year,
    string Color,
    int MakeId,
    int ModelId
);