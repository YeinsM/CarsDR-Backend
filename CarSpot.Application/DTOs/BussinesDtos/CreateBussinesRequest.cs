namespace CarSpot.Application.DTOs
{
    public record CreateBussinesRequest(
        Guid BussinesNumber,
        string? PhoneBussines,
        string? ExtencionBussines,
        string? AddreesBussines
    );
}