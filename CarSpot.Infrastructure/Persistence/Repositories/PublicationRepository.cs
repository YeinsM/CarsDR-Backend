using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

public class PublicationRepository : IPublicationRepository
{
    private readonly ApplicationDbContext _context;

    public PublicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Publication>> GetAllAsync()
    {
        return await _context.Publications
            .Include(p => p.Make)
            .Include(p => p.Model)
            .Include(p => p.Color)
            .ToListAsync();
    }

    public async Task<Publication?> GetByIdAsync(Guid id)
    {
        return await _context.Publications
            .Include(p => p.Make)
            .Include(p => p.Model)
            .Include(p => p.Color)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Publication publication)
    {
        await _context.Publications.AddAsync(publication);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Publication publication)
    {
        _context.Publications.Update(publication);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Publication publication)
    {
        _context.Publications.Remove(publication);
        await _context.SaveChangesAsync();
    }
}
