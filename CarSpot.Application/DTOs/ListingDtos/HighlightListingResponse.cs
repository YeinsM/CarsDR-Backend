namespace CarSpot.Application.DTOs
{
    public record HighlightListingResponse(
        Guid ListingId,
        bool IsHighlighted,
        DateTime? HighlightStart,
        DateTime? HighlightEnd
    );
}
