using Microsoft.AspNetCore.Http;
namespace CarSpot.Application.DTOs
{
    public class CreateVehicleImageRequest
    {

        public IFormFile File { get; set; } = default!;
        public Guid VehicleId { get; set; }
    }
}