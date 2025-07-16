using CarSpot.Domain.Common;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

public class AuxiliarRepository<T> : IAuxiliarRepository<T> where T : BaseAuxiliar
{
    private readonly ApplicationDbContext _context;

    public AuxiliarRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<T> Add(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }


    public async Task<T> DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
