public interface IPublicationRepository
{
    Task<IEnumerable<Publication>> GetAllAsync();
    Task<Publication?> GetByIdAsync(Guid id);
    Task AddAsync(Publication publication);
    Task DeleteAsync(Guid id);
}
