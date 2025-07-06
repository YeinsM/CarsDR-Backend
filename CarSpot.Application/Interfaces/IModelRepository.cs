using CarSpot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarSpot.Application.Interfaces.Repositories
{
    public interface IModelRepository
    {
        Task<Model?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Model>> GetAllAsync();
        Task Add(Model model);
        Task UpdateAsync(Guid id,  string newName, Guid newMakeId);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
