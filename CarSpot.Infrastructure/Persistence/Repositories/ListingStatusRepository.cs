using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarSpot.Infrastructure.Persistence.Repositories;

namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class ListingStatusRepository : IListingStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public ListingStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ListingStatus>> GetAllAsync()
        {
            return await _context.ListingStatuses.ToListAsync();
        }
    }
}
