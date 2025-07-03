namespace CarSpot.Application.DTOs;

public record VehicleDto(
    Guid Id,
    string VIN,
    int Year,
    string Make,
    string Model,
    Guid ModelId,
    string Color,
    string Condition,
    string Transmission,
    string Drivetrain,
    string CylinderOption,
    string CabType,
    string MarketVersion,
    string VehicleVersion,
    Guid UserId,
    List<VehicleImageDto> Images
);


