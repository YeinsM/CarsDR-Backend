using CarSpot.Domain.Entities;
using System.Threading.Tasks;

using CarSpot.Application.Interfaces;


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