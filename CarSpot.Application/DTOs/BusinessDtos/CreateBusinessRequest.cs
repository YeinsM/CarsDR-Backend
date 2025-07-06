namespace CarSpot.Application.DTOs
{
    public record CreateBusinessRequest(
        string Name,
        string BusinessNumber,
        string? Phone,
        string? Extension,
        string? Address
    );
}