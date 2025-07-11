using Microsoft.AspNetCore.Http;

public class UploadVehicleMediaFileRequest
{
    public Guid VehicleId { get; set; }
    public List<IFormFile> Files { get; set; } = new();
}
