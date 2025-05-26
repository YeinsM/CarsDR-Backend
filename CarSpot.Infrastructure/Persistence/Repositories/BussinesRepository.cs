using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class BussinesRepository : IBussinesRepository
    {
        private readonly ApplicationDbContext _context;

        public BussinesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Bussines?> GetByIdAsync(Guid id)
        {
            return await _context.Bussines!.FindAsync(id);
        }

        public async Task<IEnumerable<Bussines>> GetAllAsync()
        {
            return await _context.Bussines!.ToListAsync();
        }

        public async Task AddAsync(Bussines bussines)
        {
            await _context.Bussines!.AddAsync(bussines);
        }

        public void Update(Bussines bussines)
        {
            _context.Bussines!.Update(bussines);
        }

        public void Delete(Bussines bussines)
        {
            _context.Bussines!.Remove(bussines);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Bussines?> GetByBussinesNumberAsync(Guid bussinesNumber)
        {
            return await _context.Bussines!
                .FirstOrDefaultAsync(b => b.BussinesNumber == bussinesNumber);
        }
    }
}
