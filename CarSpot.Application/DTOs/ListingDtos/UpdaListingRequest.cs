namespace CarSpot.Application.DTOs
{
    public record UpdateListingRequest(
        Guid VehicleId,
        string Title,
        string Description,
        decimal Price,
        Guid CurrencyId,
        int ListingStatusId,
        bool IsFeatured,
        DateTime? FeaturedUntil,
        DateTime? ExpiresAt
    );
}
