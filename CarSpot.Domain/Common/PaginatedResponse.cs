namespace CarSpot.Domain.Common;

public class PaginatedResponse<T>
{
    public PaginationMetadata Pagination { get; set; }
    public IEnumerable<T> Data { get; set; }

    public PaginatedResponse(IEnumerable<T> data, int page, int pageSize, int total, string baseUrl)
    {
        Data = data;

        int totalPages = pageSize > 0 ? (int)Math.Ceiling((double)total / pageSize) : 0;

        Pagination = new PaginationMetadata
        {
            Url = baseUrl,
            Count = data.Count(),
            Total = total,
            Pages = totalPages,
            Next = page < totalPages ? $"{baseUrl}?page={page + 1}" : "",
            Prev = page > 1 ? $"{baseUrl}?page={page - 1}" : "",
            First = $"{baseUrl}?page=1",
            Last = $"{baseUrl}?page={totalPages}"
        };
    }
}

