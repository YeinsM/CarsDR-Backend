namespace CarSpot.Application.DTOs;

public record VehicleCreatedEmailDto(
    string FullName,
    string Email,
    string VehicleTitle,
    string VIN,
    string Make,
    string Model,
    int Year
);
