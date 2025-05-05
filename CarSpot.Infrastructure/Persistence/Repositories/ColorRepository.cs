using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using CarSpot.Application.Interfaces;

public class ColorRepository : IColorRepository
{
    private readonly ApplicationDbContext _context;

    public ColorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Color>> GetAllAsync()
    {
        return await _context.Colors.ToListAsync();
    }

    public async Task<Color?> GetByIdAsync(Guid id)
    {
        return await _context.Colors.FindAsync(id);
    }

    public async Task AddAsync(Color color)
    {
        await _context.Colors.AddAsync(color);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Color color)
    {
        _context.Colors.Update(color);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Color color)
    {
        _context.Colors.Remove(color);
        await _context.SaveChangesAsync();
    }

   
}
