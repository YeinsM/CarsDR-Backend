using System.Collections.Generic;
using System.Threading.Tasks;
using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces
{
    public interface IMenuRepository
    {
        Task<Menu> GetByIdAsync(int id);
        Task<IEnumerable<Menu>> GetAllAsync();
        Task AddAsync(Menu menu);
        Task Update(Menu menu);
        Task DeleteAsync(int id);
        
        Task<bool> ExistsAsync(int id);
    }
}