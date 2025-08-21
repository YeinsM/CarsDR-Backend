
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

public class VehicleMediaFileRepository(ApplicationDbContext context) : IVehicleMediaFileRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<VehicleMediaFile?> GetByIdAsync(Guid id)
        => await _context.VehicleMediaFiles.FirstOrDefaultAsync(m => m.Id == id);

    public async Task<IEnumerable<VehicleMediaFile>> GetByVehicleIdAsync(Guid vehicleId)
        => await _context.VehicleMediaFiles.Where(m => m.VehicleId == vehicleId).ToListAsync();

    public async Task AddAsync(VehicleMediaFile media)
    {
        await _context.VehicleMediaFiles.AddAsync(media);
    }

    public IQueryable<VehicleMediaFile> Query()
    {
        return _context.VehicleMediaFiles.AsQueryable();
    }



    public async Task DeleteAsync(Guid id)
    {
        VehicleMediaFile? media = await GetByIdAsync(id);
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
