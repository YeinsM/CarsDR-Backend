using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ApplicationDbContext _context;

        public CurrencyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

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
        }

        public Task Update(Currency currency)
        {
            _context.Currencies.Update(currency);
            return Task.CompletedTask;
        }

        public async Task Delete(Guid id)
        {
            var currency = await _context.Currencies.FindAsync(id);
            if (currency != null)
            {
                _context.Currencies.Remove(currency);
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Currency> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Currencies.AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
