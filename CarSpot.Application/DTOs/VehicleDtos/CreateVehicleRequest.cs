namespace CarSpot.Application.DTOs
{
    public record CreateVehicleRequest(
        string VIN,
        Guid UserId,
        int MakeId,
        int ModelId,
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
}

