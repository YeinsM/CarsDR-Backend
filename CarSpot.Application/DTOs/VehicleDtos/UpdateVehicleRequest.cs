namespace CarSpot.Application.DTOs;

public record UpdateVehicleRequest(
        Guid MakeId,
        Guid ModelId,
        int VehicleVersionId,
        int MarketVersionId,
        int TransmissionId,
        int DrivetrainId,
        int CylinderOptionId,
        int CabTypeId,
        int ConditionId,
        int ColorId,
        int Year,
        int Mileage,
        decimal Price,
        string Title,
        bool IsFeatured,
        DateTime FeaturedUntil
    );