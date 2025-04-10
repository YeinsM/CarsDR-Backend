using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using CarSpot.Application.Interfaces;
using CarSpot.Infrastructure.Persistence.Context;


namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class MakeRepository : IMakeRepository
    {
        private readonly ApplicationDbContext _context;

        public MakeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Make>> GetAllAsync()
        {
            return await _context.Makes
                .Include(m => m.Models)
                .ToListAsync();
        }

        public async Task<Make?> GetByIdAsync(int id)
        {
            return await _context.Makes
                .Include(m => m.Models)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Make make)
        {
            _context.Makes.Add(make);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Make make)
        {
            _context.Makes.Update(make);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Make make)
        {
            _context.Makes.Remove(make);
            await _context.SaveChangesAsync();
        }
    }
}