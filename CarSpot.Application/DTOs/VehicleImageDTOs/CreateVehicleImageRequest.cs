using Microsoft.AspNetCore.Http;
namespace CarSpot.Application.DTOs.VehicleImage;
public record CreateVehicleImageRequest
(Guid VehicleId, IFormFile ImageFile);