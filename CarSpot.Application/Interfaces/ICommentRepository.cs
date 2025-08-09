

namespace CarSpot.Application.Interfaces
{
    public interface ICommentRepository
    {
        
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(Guid id);
        Task<IEnumerable<Comment>> GetByListingIdAsync(Guid listingId);
        Task<IEnumerable<Comment>> GetByUserIdAsync(Guid userId);
        Task CreateAddAsync(Comment comment);
        Task DeleteAsync(Comment comment);
        Task SaveChangesAsync();

        // Métodos optimizados con paginación
        Task<IEnumerable<Comment>> GetByListingIdPagedAsync(Guid listingId, int pageNumber, int pageSize);
        Task<int> CountByListingIdAsync(Guid listingId);

        Task<IEnumerable<Comment>> GetByUserIdPagedAsync(Guid userId, int pageNumber, int pageSize);
        Task<int> CountByUserIdAsync(Guid userId);
    }
}
