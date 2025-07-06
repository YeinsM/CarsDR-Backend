namespace CarSpot.Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(Guid id);
        Task<IEnumerable<Comment>> GetByVehicleIdAsync(Guid vehicleId);
        Task<IEnumerable<Comment>> GetByCommentIdAsync(Guid userId);
        Task CreateAddAsync(Comment comment);
        Task DeleteAsync(Comment comment);
        Task SaveChangesAsync();
    }
}
