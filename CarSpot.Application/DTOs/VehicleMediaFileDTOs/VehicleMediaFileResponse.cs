namespace CarSpot.Application.DTOs;

public record VehicleMediaFileResponse(
    Guid Id,
    Guid VehicleId,
    string Url
);
