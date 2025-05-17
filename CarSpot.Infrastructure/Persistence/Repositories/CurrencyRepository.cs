using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence;
using CarSpot.Infrastructure.Persistence.Context;



namespace CarSpot.Infrastructure.Persistence.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly ApplicationDbContext _context;

    public CurrencyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Currency> GetAll() => _context.Currencies.ToList();

    public Currency? GetById(Guid id) => _context.Currencies.Find(id);

    public void Add(Currency currency)
    {
        _context.Currencies.Add(currency);
        _context.SaveChanges();
    }

    public void Update(Currency currency)
    {
        _context.Currencies.Update(currency);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var currency = _context.Currencies.Find(id);
        if (currency != null)
        {
            _context.Currencies.Remove(currency);
            _context.SaveChanges();
        }
    }
}
