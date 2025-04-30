using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using CarSpot.Application.Interfaces;
using CarSpot.Infrastructure.Persistence.Context;


namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class ModelRepository(ApplicationDbContext _context) : IRepository<Model>
    {
        public async Task<IEnumerable<Model>> GetAllAsync()
        {
            return await _context.Models
                .Include(m => m.Make)
                .Include(m => m.Vehicles)
                .ToListAsync();
        }

        public async Task<Model?> GetByIdAsync(int id)
        {
            return await _context.Models
                .Include(m => m.Make)
                .Include(m => m.Vehicles)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Model model)
        {
            await _context.Models.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Model model)
        {
            _context.Models.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Model model)
        {
            _context.Models.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
