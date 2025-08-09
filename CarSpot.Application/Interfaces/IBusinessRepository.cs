using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces
{
    public interface IBusinessRepository
    {
        Task<Business?> GetByIdAsync(Guid id);
        Task<IEnumerable<Business>> GetAllAsync();
        Task AddAsync(Business bussines);
        void Update(Business bussines);
        void Delete(Business bussines);
        Task<int> SaveChangesAsync();
        Task<Business?> GetByBussinesNumberAsync(Guid bussinesNumber);
    }
}
