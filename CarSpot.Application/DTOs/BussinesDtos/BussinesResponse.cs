 namespace CarSpot.Application.DTOs
{
 
 public record BussinesResponse(
        Guid Id,
        Guid BussinesNumber,
        string? PhoneBussines,
        string? ExtencionBussines,
        string? AddreesBussines
    );
}