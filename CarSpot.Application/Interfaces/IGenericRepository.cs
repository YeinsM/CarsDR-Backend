using CarSpot.Domain.Common;

namespace CarSpot.Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
       
    }
}
