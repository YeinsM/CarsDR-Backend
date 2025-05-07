using CarSpot.Domain.Entities;
using CarSpot.Application.Common.Interfaces;
using CarSpot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Repositories
{
    public class VehicleImageRepository : IVehicleImageRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VehicleImage>> GetAllAsync()
        {
            return await _context.VehicleImages
                .Include(img => img.Vehicle)
                .ToListAsync();
        }

        public async Task<VehicleImage?> GetByIdAsync(Guid id)
        {
            return await _context.VehicleImages
                .Include(img => img.Vehicle)
                .FirstOrDefaultAsync(img => img.Id == id);
        }

        public async Task<IEnumerable<VehicleImage>> GetByVehicleIdAsync(Guid vehicleId)
        {
            return await _context.VehicleImages
                .Where(img => img.VehicleId == vehicleId)
                .ToListAsync();
        }

        public async Task AddAsync(VehicleImage image)
        {
            await _context.VehicleImages.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var image = await _context.VehicleImages.FindAsync(id);
            if (image != null)
            {
                _context.VehicleImages.Remove(image);
                await _context.SaveChangesAsync();
            }
        }
    }
}
