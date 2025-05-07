using CarSpot.Domain.Common;

public interface IAuxiliarRepository<T> where T : BaseAuxiliar
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    void Remove(T entity);
}
