namespace CarSpot.Application.DTOs.Vehicle
{
    public record CreateVehicleRequest(
        string VIN,
        Guid UserId,
        Guid MakeId,
        Guid ModelId,
        Guid? VersionId,
        Guid? MarketVersionId,
        Guid? TransmissionId,
        Guid? DrivetrainId,
        Guid? CylinderOptionId,
        Guid? CabTypeId,
        Guid ConditionId,
        Guid? ColorId,
        int Year,
        int? Mileage,
        decimal Price,
        string? Title,
        bool IsFeatured,
        DateTime? FeaturedUntil
    );
}

