using CarSpot.Domain.Common;

namespace CarSpot.Application.Interfaces
{
    public interface IRepository<T> where T : BaseEntity // Fix: Add constraint to ensure T inherits from BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
