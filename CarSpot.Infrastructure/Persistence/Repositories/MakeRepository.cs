using CarSpot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CarSpot.Application.Interfaces;
using CarSpot.Infrastructure.Persistence.Context;


namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class MakeRepository(ApplicationDbContext context) : IRepository<Make>
    {
        public async Task<IEnumerable<Make>> GetAllAsync()
        {
            return await context.Makes
                .Include(m => m.Models)
                .ToListAsync();
        }

        public async Task<Make?> GetByIdAsync(Guid id)
        {
            return await context.Makes
                .Include(m => m.Models)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Make make)
        {
            await context.Makes.AddAsync(make);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Make make)
        {
            context.Makes.Update(make);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Make make)
        {
            context.Makes.Remove(make);
            await context.SaveChangesAsync();
        }
    }
}