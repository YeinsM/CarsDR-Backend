using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces;

public interface ICurrencyRepository
{
    Task<(IEnumerable<Currency> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    Task<Currency?> GetById(Guid id);
    Task Add(Currency currency);
    Task Update(Currency currency);
    Task Delete(Guid id);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

