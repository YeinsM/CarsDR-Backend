namespace CarSpot.Application.DTOs.BusinessDtos
{

    public record BusinessResponse(
           Guid Id,
           string Name,
           string BussinesNumber,
           string? PhoneBussines,
           string? ExtencionBussines,
           string? AddreesBussines
       );
}
