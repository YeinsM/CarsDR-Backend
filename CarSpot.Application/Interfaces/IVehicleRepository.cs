using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Vehicle vehicle);
    }
}
