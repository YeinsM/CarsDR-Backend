using CarSpot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarSpot.Application.Interfaces.Repositories
{
    public interface IBussinesRepository
    {
        Task<Bussines?> GetByIdAsync(Guid id);
        Task<IEnumerable<Bussines>> GetAllAsync();
        Task AddAsync(Bussines bussines);
        void Update(Bussines bussines);
        void Delete(Bussines bussines);
        Task<int> SaveChangesAsync();
        Task<Bussines?> GetByBussinesNumberAsync(Guid bussinesNumber);
    }
}
