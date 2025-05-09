using CarSpot.Domain.Entities;
using CarSpot.Application.Interfaces;
using CarSpot.Infrastructure.Persistence;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Repositories
{
    public class CommentRepository : IAuxiliarRepository<Comment>
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Vehicle)
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Vehicle)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment> Add(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

       public async Task<Comment> DeleteAsync(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return null!;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}
