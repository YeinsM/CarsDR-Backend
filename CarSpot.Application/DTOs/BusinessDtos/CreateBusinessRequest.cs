namespace CarSpot.Application.DTOs
{
    public record CreateBusinessRequest(
        Guid BusinessNumber,
        string? Phone,
        string? Extension,
        string? Address
    );
}