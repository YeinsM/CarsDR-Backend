using CarSpot.Application.Common.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Repositories
{
    public class PublicationRepository : IPublicationRepository
    {
        private readonly ApplicationDbContext _context;

        public PublicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Publication>> GetAllAsync()
        {
            return await _context.Publications
                .Include(p => p.User)
                .Include(p => p.Make)
                .Include(p => p.Model)
                .Include(p => p.Color)
                .ToListAsync();
        }

        public async Task<Publication?> GetByIdAsync(Guid id)
        {
            return await _context.Publications
                .Include(p => p.User)
                .Include(p => p.Make)
                .Include(p => p.Model)
                .Include(p => p.Color)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Publication publication)
        {
            await _context.Publications.AddAsync(publication);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var pub = await _context.Publications.FindAsync(id);
            if (pub != null)
            {
                _context.Publications.Remove(pub);
                await _context.SaveChangesAsync();
            }
        }
    }
}
