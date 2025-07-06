using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence;
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
            return await _context.Comments!.ToListAsync();
        }
        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await _context.Comments!
                .Include(c => c.UserId)
                .Include(c => c.VehicleId)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetByVehicleIdAsync(Guid vehicleId)
        {
            return await _context.Comments!
                .Where(c => c.VehicleId == vehicleId)
                .Include(c => c.UserId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByCommentIdAsync(Guid userId)
        {
            return await _context.Comments!
                .Where(c => c.UserId == userId)
                .Include(c => c.VehicleId)
                .ToListAsync();
        }

        public async Task CreateAddAsync(Comment comment)
        {
            _context.Comments!.Add(comment);
            await SaveChangesAsync();
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
