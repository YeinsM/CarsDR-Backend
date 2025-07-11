namespace CarSpot.Application.DTOs;

public record VehicleDto(
    Guid Id,
    string VIN,
    decimal Price,
    string Title,
    bool IsFeatured,
    DateTime? FeaturedUntil,
    int Mileage,
    int Year,
    string VehicleType,
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
    List<VehicleMediaFileDto> MediaFile
);
