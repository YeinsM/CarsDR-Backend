
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Services
{
    public class PaginationService : IPaginationService
    {
        public async Task<PaginatedResponse<T>> PaginateAsync<T>(
            IQueryable<T> query,
            int pageNumber,
            int pageSize,
            string baseUrl = "")
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

    
            return new PaginatedResponse<T>(
                data: items,
                page: pageNumber,
                pageSize: pageSize,
                total: totalRecords,
                baseUrl: baseUrl
            );
        }
    }
}
