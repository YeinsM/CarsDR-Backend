public class AuxiliarRepository<T> : IAuxiliarRepository<T> where T : BaseAuxiliar
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public AuxiliarRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public async Task UpdateAsync(T entity) => _dbSet.Update(entity);

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
            _dbSet.Remove(entity);
    }
}
