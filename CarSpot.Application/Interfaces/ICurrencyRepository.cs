using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces;

public interface ICurrencyRepository
{
    
     IQueryable<Currency> Query();
    Task<Currency?> GetById(Guid id);
    Task Add(Currency currency);
    Task Update(Currency currency);
    Task Delete(Guid id);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

