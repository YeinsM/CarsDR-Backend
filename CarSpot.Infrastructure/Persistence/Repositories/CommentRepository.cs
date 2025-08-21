using CarSpot.Application.Interfaces;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Repositories
{
    public class CommentRepository(ApplicationDbContext context) : ICommentRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments!
                .Include(c => c.User)
                .Include(c => c.Listing)
                .OrderByDescending(c => c.CreatedAt)
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

        public async Task<IEnumerable<Comment>> GetByListingIdPagedAsync(Guid listingId, int pageNumber, int pageSize)
        {
            return await _context.Comments!
                .Include(c => c.User)
                .Where(c => c.ListingId == listingId)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountByListingIdAsync(Guid listingId)
        {
            return await _context.Comments!
                .CountAsync(c => c.ListingId == listingId);
        }

        public async Task<IEnumerable<Comment>> GetByUserIdPagedAsync(Guid userId, int pageNumber, int pageSize)
        {
            return await _context.Comments!
                .Include(c => c.Listing)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountByUserIdAsync(Guid userId)
        {
            return await _context.Comments!
                .CountAsync(c => c.UserId == userId);
        }

        public IQueryable<Comment> Query()
        {
            return _context.Comments!
                .Include(c => c.User)
                .Include(c => c.Listing)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();
        }

        public IQueryable<Comment> QueryByListingId(Guid listingId)
        {
            return _context.Comments!
                .Include(c => c.User)
                .Where(c => c.ListingId == listingId)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();
        }

        public IQueryable<Comment> QueryByUserId(Guid userId)
        {
            return _context.Comments!
                .Include(c => c.Listing)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();
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
