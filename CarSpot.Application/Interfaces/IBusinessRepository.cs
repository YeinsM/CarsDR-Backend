using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces.Repositories
{
    public interface IBusinessRepository
    {
        Task<Business?> GetByIdAsync(Guid id);
        Task<IEnumerable<Business>> GetAllAsync();
        IQueryable<Business> Query();
        Task Add(Business bussines);
        void Update(Business bussines);
        void Delete(Business bussines);
        Task<int> SaveChangesAsync();
        Task<Business?> GetByBussinesNumberAsync(string bussinesNumber);
    }
}
