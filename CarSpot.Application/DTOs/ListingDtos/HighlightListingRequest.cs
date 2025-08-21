namespace CarSpot.Application.DTOs
{
    public record HighlightListingRequest(
        Guid ListingId,
        DateTime StartDate,
        DateTime EndDate
    );
}