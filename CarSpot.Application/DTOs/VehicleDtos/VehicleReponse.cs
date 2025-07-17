public record VehicleResponse(
    Guid Id,
    string Title,
    string VIN,
    string VehicleTypeName,
    string Make,
    string Model,
    string Condition,
    int Year,
    decimal Price
);
