using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class ListingRepository : IListingRepository

    {
        private readonly ApplicationDbContext _context;

        public ListingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Listing>> GetAllAsync()
        {
            return await _context.Listings
                .Include(p => p.User)
                .Include(p => p.Vehicle)
                .ToListAsync();
        }

        public async Task<Listing?> GetByIdAsync(Guid id)
        {
            return await _context.Listings
                .Include(p => p.User)
                .Include(p => p.Vehicle)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Listing> Add(Listing Listing)
        {
            _context.Listings.Add(Listing);
            await _context.SaveChangesAsync();
            return Listing;
        }

        public async Task<Listing> UpdateAsync(Listing Listing)
        {
            _context.Listings.Update(Listing);
            await _context.SaveChangesAsync();
            return Listing;
        }

        public async Task<Listing> DeleteAsync(Guid id)
        {
            var Listing = await _context.Listings.FindAsync(id);
            if (Listing == null)
                return null!;

            _context.Listings.Remove(Listing);
            await _context.SaveChangesAsync();
            return Listing;
        }
    }
}
