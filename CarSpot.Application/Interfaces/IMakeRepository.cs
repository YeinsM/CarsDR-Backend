using CarSpot.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using CarSpot.Application.Interfaces;

namespace CarSpot.Application.Interfaces
{
    public interface IMakeRepository
    {
        Task<IEnumerable<Make>> GetAllAsync();
        Task<Make?> GetByIdAsync(Guid id);
        Task AddAsync(Make make);
        Task UpdateAsync(Make make);
        Task DeleteAsync(Make make);
    }
}
