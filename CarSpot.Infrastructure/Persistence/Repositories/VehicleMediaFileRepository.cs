using CarSpot.Domain.Common;
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

    public async Task<PaginatedResponse<VehicleMediaFile>> GetByVehicleIdPagedAsync(
    Guid vehicleId, int page, int pageSize, string baseUrl)
    {
        
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

     
        var query = _context.VehicleMediaFiles
            .Where(m => m.VehicleId == vehicleId)
            .AsNoTracking();

        
        var totalItems = await query.CountAsync();

      
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        
        return new PaginatedResponse<VehicleMediaFile>(
            data: items,
            page: page,
            pageSize: pageSize,
            total: totalItems,
            baseUrl: baseUrl
        );
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
