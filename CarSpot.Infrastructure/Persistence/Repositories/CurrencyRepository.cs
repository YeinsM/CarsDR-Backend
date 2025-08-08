using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Persistence.Repositories;

public class CurrencyRepository(ApplicationDbContext context) : ICurrencyRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Currency>> GetAll()
    {
        return await _context.Currencies.ToListAsync();
    }

    public async Task<Currency?> GetById(Guid id)
    {
        return await _context.Currencies.FindAsync(id);
    }

    public async Task Add(Currency currency)
    {
        await _context.Currencies.AddAsync(currency);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Currency currency)
    {
        _context.Currencies.Update(currency);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        Currency? currency = await _context.Currencies.FindAsync(id);
        if (currency != null)
        {
            _context.Currencies.Remove(currency);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
