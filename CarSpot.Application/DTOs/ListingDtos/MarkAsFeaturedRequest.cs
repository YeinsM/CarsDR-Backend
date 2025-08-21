namespace CarSpot.Application.DTOs
{
    public record MarkAsFeaturedRequest(
        DateTime StartDate,
        DateTime EndDate
    );
}
