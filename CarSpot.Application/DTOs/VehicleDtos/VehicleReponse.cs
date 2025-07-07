public record VehicleResponse(
    Guid Id,
    string Title,
    string VIN,
    string VehicleTypeName,
    string Make,
    string Model,
    string Condition,
    string Color,
    int Year,
    decimal Price
);
