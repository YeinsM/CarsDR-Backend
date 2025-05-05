using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CarSpot.Infrastructure.Persistence.Context;

namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class VehicleRepository(ApplicationDbContext _context) : IRepository<Vehicle>
    {
        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles
                .Include(v => v.Model)
                .ThenInclude(m => m.Make)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id)
        {
            return await _context.Vehicles
                .Include(v => v.Model)
                .ThenInclude(m => m.Make)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }
    }
}

