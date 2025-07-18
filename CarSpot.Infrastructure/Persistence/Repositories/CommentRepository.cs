using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments!
                .Include(c => c.User)
                .Include(c => c.Listing)
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await _context.Comments!
                .Include(c => c.User)
                .Include(c => c.Listing)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetByListingIdAsync(Guid listingId)
        {
            return await _context.Comments!
                .Include(c => c.User)
                .Where(c => c.ListingId == listingId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Comments!
                .Include(c => c.Listing)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task CreateAddAsync(Comment comment)
        {
            await _context.Comments!.AddAsync(comment);
        }

        public async Task DeleteAsync(Comment comment)
        {
            _context.Comments!.Remove(comment);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
