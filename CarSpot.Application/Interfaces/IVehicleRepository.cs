using CarSpot.Application.DTOs;
using CarSpot.Domain.Entities;
using CarSpot.Domain.Common;


namespace CarSpot.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<VehicleDto>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Vehicle vehicle);
         Task<PaginatedResponse<Vehicle>> FilterAsync(VehicleFilterRequest request);

     
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        IQueryable<Vehicle> Query();

    }
}
