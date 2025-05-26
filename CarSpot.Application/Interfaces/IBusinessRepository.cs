using CarSpot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarSpot.Application.Interfaces.Repositories
{
    public interface IBusinessRepository
    {
        Task<Business?> GetByIdAsync(Guid id);
        Task<IEnumerable<Business>> GetAllAsync();
        Task AddAsync(Business bussines);
        void Update(Business bussines);
        void Delete(Business bussines);
        Task<int> SaveChangesAsync();
        Task<Business?> GetByBussinesNumberAsync(Guid bussinesNumber);
    }
}
