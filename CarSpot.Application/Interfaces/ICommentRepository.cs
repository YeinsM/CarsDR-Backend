namespace CarSpot.Application.Interfaces.Repositories
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
    }
}
