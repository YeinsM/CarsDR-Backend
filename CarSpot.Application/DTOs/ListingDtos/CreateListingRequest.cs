namespace CarSpot.Application.DTOs
{
    public record CreateListingRequest(
        Guid UserId,
        Guid VehicleId,
        string Title,
        string Description,
        decimal? ListingPrice,
        decimal Price,
        Guid CurrencyId,
        int ListingStatusId,
        DateTime? ExpiresAt,
        bool IsFeatured,
        DateTime? FeaturedUntil,
        int ViewCount,
         List<string> Images
        

    );
}
