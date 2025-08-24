namespace CarSpot.Application.DTOs.BusinessDtos
{
    public record CreateBusinessRequest(
        string Name,
        string BusinessNumber,
        string? Phone,
        string? Extension,
        string? Address
    );
}
