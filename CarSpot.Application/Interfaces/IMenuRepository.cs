using System.Collections.Generic;
using System.Threading.Tasks;
using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces
{
    public interface IMenuRepository
    {
        Task<Menu> GetByIdAsync(Guid id);
        Task<IEnumerable<Menu>> GetAllAsync();
        Task AddAsync(Menu menu);
        Task Update(Menu menu);
        Task DeleteAsync(Guid id);
        
        Task<bool> ExistsAsync(Guid id);
    }
}