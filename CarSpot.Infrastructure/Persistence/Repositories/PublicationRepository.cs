using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Repositories
{
    public class PublicationRepository : IAuxiliarRepository<Publication>

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

        public async Task<Publication> Add(Publication publication)
        {
            _context.Publications.Add(publication);
            await _context.SaveChangesAsync();
            return publication;
        }

        public async Task<Publication> UpdateAsync(Publication publication)
        {
            _context.Publications.Update(publication);
            await _context.SaveChangesAsync();
            return publication;
        }

        public async Task<Publication> DeleteAsync(Guid id)
        {
            var publication = await _context.Publications.FindAsync(id);
            if (publication == null)
                return null!;

            _context.Publications.Remove(publication);
            await _context.SaveChangesAsync();
            return publication;
        }
    }
}
