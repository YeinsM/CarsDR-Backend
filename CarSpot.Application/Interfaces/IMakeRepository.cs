using CarSpot.Domain.Entities;

using CarSpot.Application.Interfaces;

namespace CarSpot.Application.Interfaces
{
    public interface IMakeRepository
    {
        Task<IEnumerable<Make>> GetAllAsync();
        Task<Make?> GetByIdAsync(int id);
        Task AddAsync(Make make);
        Task UpdateAsync(Make make);
        Task DeleteAsync(Make make);
    }
}
