namespace CarSpot.Application.DTOs
{

 public record UpdateBusinessRequest(
        Guid Id,
        Guid BusinessNumber,
        string? Phone,
        string? Extension,
        string? Address
    );
}