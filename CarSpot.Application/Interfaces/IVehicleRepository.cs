using CarSpot.Domain;
using CarSpot.Domain.Entities;



namespace CarSpot.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(int id);
        Task<Vehicle> AddAsync(Vehicle vehicle);
        Task<int> SaveChangesAsync();
       


    }
}
