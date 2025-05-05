public interface IPublicationRepository
{
    Task<List<Publication>> GetAllAsync();
    Task<Publication?> GetByIdAsync(Guid id);
    Task AddAsync(Publication publication);
    Task UpdateAsync(Publication publication);
    Task DeleteAsync(Publication publication);
}
