using CarSpot.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarSpot.Application.Interfaces.Repositories
{
    public interface IMakeRepository
    {
        Task<Make> GetByIdAsync(Guid id);
        Task<IEnumerable<Make>> GetAllAsync();
        Task Add(Make make);
        Task UpdateAsync(Guid id, string newName);
        Task RemoveAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
