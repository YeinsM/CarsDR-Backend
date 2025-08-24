namespace CarSpot.Application.DTOs.BusinessDtos
{

    public record UpdateBusinessRequest(
           Guid Id,
           string Name,
           string BusinessNumber,
           string? Phone,
           string? Extension,
           string? Address
       );
}
