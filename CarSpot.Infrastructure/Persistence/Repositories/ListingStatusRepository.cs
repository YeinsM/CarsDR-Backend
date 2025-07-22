using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class ListingStatusRepository(ApplicationDbContext context) : IListingStatusRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<ListingStatus>> GetAllAsync()
        {
            return await _context.ListingStatuses.ToListAsync();
        }
    }
}
