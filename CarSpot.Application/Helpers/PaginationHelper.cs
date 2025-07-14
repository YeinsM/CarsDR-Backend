
using Microsoft.EntityFrameworkCore;
using CarSpot.Application.Common.Extensions;


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

        int totalPages = (int)Math.Ceiling(total / (double)pageSize);

        var buildUrl = (int targetPage) =>
            $"{baseUrl}?page={targetPage}&pageSize={pageSize}&orderBy={orderBy}&sortDir={sortDir}";

        return new PaginatedResponse<T>
        {
            Collection = new CollectionMetadata
            {
                Url = buildUrl(page),
                Count = data.Count,
                Pages = totalPages,
                Total = total,
                First = buildUrl(1),
                Last = buildUrl(totalPages),
                Next = page < totalPages ? buildUrl(page + 1) : null,
                Prev = page > 1 ? buildUrl(page - 1) : null
            },
            Data = data
        };
    }
}
