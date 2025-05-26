namespace CarSpot.Application.DTOs
{

 public record UpdateBussinesRequest(
        Guid Id,
        Guid BussinesNumber,
        string? PhoneBussines,
        string? ExtencionBussines,
        string? AddreesBussines
    );
}