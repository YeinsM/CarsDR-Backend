using CarSpot.Domain.Entities;


namespace CarSpot.Application.Interfaces.Repositories
{
    public interface IMakeRepository
    {
        IQueryable<Make> Query();

        Task<Make> GetByIdAsync(Guid id);
        Task<IEnumerable<Make>> GetAllAsync();
        Task Add(Make make);
        Task UpdateAsync(Guid id, string newName);
        Task RemoveAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
