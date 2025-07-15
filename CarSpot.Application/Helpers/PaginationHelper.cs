using CarSpot.Application.Common.Extensions;
using CarSpot.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Application.Common.Helpers;

public static class PaginationHelper
{
    public static async Task<PaginatedResponse<T>> CreatePaginatedResponse<T>(
        IQueryable<T> query,
        int page,
        int pageSize,
        string baseUrl,
        string? orderBy = null,
        string? sortDir = "asc")
    {
        int total = await query.CountAsync();

        if (!string.IsNullOrEmpty(orderBy))
        {
            query = query.OrderByDynamic(orderBy, sortDir);
        }

        var data = await query.Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();

        return new PaginatedResponse<T>(data, page, pageSize, total, baseUrl);
    }
}
