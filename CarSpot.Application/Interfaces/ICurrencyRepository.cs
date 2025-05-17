using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces;

public interface ICurrencyRepository
{
    IEnumerable<Currency> GetAll();
    Currency? GetById(Guid id);
    void Add(Currency currency);
    void Update(Currency currency);
    void Delete(Guid id);
}
