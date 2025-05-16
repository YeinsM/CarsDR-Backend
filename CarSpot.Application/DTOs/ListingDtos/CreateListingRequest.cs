namespace CarSpot.Application.DTOs
{
    public record CreateListingRequest(
        Guid UserId,
        Guid VehicleId,
        string Title,
        string Description,
        decimal Price,
        Guid CurrencyId,
        int ListingStatusId
    );
}
