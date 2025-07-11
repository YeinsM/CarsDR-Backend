using CarSpot.Application.Interfaces;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

public class VehicleMediaFileRepository : IVehicleMediaFileRepository
{
    private readonly ApplicationDbContext _context;

    public VehicleMediaFileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleMediaFile?> GetByIdAsync(Guid id)
        => await _context.VehicleMediaFiles.FirstOrDefaultAsync(m => m.Id == id);

    public async Task<IEnumerable<VehicleMediaFile>> GetByVehicleIdAsync(Guid vehicleId)
        => await _context.VehicleMediaFiles.Where(m => m.VehicleId == vehicleId).ToListAsync();

    public async Task AddAsync(VehicleMediaFile media)
    {
        await _context.VehicleMediaFiles.AddAsync(media);
    }

    public async Task DeleteAsync(Guid id)
    {
        var media = await GetByIdAsync(id);
        if (media != null)
        {
            _context.VehicleMediaFiles.Remove(media);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
