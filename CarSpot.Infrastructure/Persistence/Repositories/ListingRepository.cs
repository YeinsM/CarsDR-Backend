
using CarSpot.Domain.Entities;
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

        public IQueryable<Listing> Query()
        {
     
            return _context.Listings
                .Include(p => p.User)
                .Include(p => p.Vehicle)
                .AsQueryable();
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

        public async Task<Listing> Add(Listing listing)
        {
            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();
            return listing;
        }

        public async Task UpdateAsync(Listing listing)
        {
            _context.Listings.Update(listing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var listing = await _context.Listings.FindAsync(id);
            if (listing != null)
            {
                _context.Listings.Remove(listing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

         public async Task<IEnumerable<Listing>> GetBySellerIdAsync(Guid sellerId)
        {
            return await _context.Listings
                .Include(l => l.Vehicle)  
                .Include(l => l.MediaFiles)    
                .Include(l => l.Currency) 
                .Where(l => l.UserId == sellerId)
                .ToListAsync();
        }
    }
}
