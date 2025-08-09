using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly ApplicationDbContext _context;

        public BusinessRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Business?> GetByIdAsync(Guid id)
        {
            return await _context.Business!.FindAsync(id);
        }

        public async Task<IEnumerable<Business>> GetAllAsync()
        {
            return await _context.Business!.ToListAsync();
        }

        public async Task AddAsync(Business bussines)
        {
            await _context.Business!.AddAsync(bussines);
        }

        public void Update(Business bussines)
        {
            _context.Business!.Update(bussines);
        }

        public void Delete(Business bussines)
        {
            _context.Business!.Remove(bussines);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Business?> GetByBussinesNumberAsync(Guid bussinesNumber)
        {
            return await _context.Business!
                .FirstOrDefaultAsync(b => b.BusinessNumber == bussinesNumber);
        }
    }
}
