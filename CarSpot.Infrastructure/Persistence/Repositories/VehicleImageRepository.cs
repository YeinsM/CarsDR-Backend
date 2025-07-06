using CarSpot.Application.Interfaces;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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

        public async Task<VehicleImage> CreateAsync(VehicleImage vehicleImage)
        {
            _context.VehicleImages.Add(vehicleImage);
            await _context.SaveChangesAsync();
            return vehicleImage;
        }

        public async Task<VehicleImage> UpdateAsync(VehicleImage vehicleImage)
        {
            _context.VehicleImages.Update(vehicleImage);
            await _context.SaveChangesAsync();
            return vehicleImage;
        }


        public async Task<VehicleImage> DeleteAsync(Guid id)
        {
            var image = await _context.VehicleImages.FindAsync(id);
            if (image == null)
                throw new KeyNotFoundException($"VehicleImage with ID {id} not found");

            _context.VehicleImages.Remove(image);
            await _context.SaveChangesAsync();
            return image;
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
