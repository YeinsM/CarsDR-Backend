using CarSpot.Domain.Common;

public interface IAuxiliarRepository<T> where T : BaseAuxiliar
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<T> Add(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(Guid id);
    
}



