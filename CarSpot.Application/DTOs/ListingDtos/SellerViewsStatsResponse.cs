namespace CarSpot.Application.DTOs
{
    public record SellerViewsStatsResponse(
        Guid SellerId,
        int TotalViews,
        IEnumerable<ListingViewDetail> Listings
    );

    
}
