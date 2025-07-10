using CarSpot.Application.DTOs;
using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<VehicleDto>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Vehicle vehicle);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
