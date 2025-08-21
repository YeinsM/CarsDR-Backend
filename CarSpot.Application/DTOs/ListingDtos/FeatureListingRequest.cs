namespace CarSpot.Application.DTOs
{
    public record FeatureListingRequest(
        DateTime StartDate,
        DateTime EndDate
    );
}
