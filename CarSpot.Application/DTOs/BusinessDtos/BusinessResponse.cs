 namespace CarSpot.Application.DTOs
{
 
 public record BusinessResponse(
        Guid Id,
        Guid BussinesNumber,
        string? PhoneBussines,
        string? ExtencionBussines,
        string? AddreesBussines
    );
}