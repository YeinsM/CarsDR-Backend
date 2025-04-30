using CarSpot.Domain.Entities;


namespace CarSpot.Application.Interfaces
{


    public interface IModelRepository
    {
        Task<IEnumerable<Model>> GetAllAsync();
        Task<Model?> GetByIdAsync(int id);
        Task AddAsync(Model model);
        Task UpdateAsync(Model model);
        Task DeleteAsync(Model model);
    }
}