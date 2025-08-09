using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Repositories
{
    public class MakeRepository(ApplicationDbContext context) : IMakeRepository
    {
        private readonly ApplicationDbContext _context = context;

        public IQueryable<Make> Query()
        {
            return _context.Makes.AsNoTracking();
        }

        public async Task<Make> GetByIdAsync(Guid id)
        {
            Make? make = await _context.Makes
                .Include(m => m.Models)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (make == null)
                throw new KeyNotFoundException($"Make with id {id} not found.");

            return make;
        }

        public async Task<IEnumerable<Make>> GetAllAsync()
        {
            return await _context.Makes
                .Include(m => m.Models)
                .ToListAsync();
        }

        public async Task Add(Make make)
        {
            await _context.Makes.AddAsync(make);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, string newName)
        {
            Make? make = await _context.Makes.FindAsync(id);

            if (make == null)
                throw new Exception("Make not found");

            make.Name = newName;

            _context.Makes.Update(make);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            Make? make = await _context.Makes.FindAsync(id);

            if (make == null)
                throw new Exception("Make not found");

            _context.Makes.Remove(make);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Makes.AnyAsync(m => m.Id == id);
        }
    }
}
