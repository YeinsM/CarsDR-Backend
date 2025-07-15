namespace CarSpot.Domain.Common;

public class PaginatedResponse<T>
{
    public CollectionMetadata Collection { get; set; }
    public IEnumerable<T> Data { get; set; }

    public PaginatedResponse(IEnumerable<T> data, int page, int pageSize, int total, string baseUrl)
    {
        Data = data;

        var totalPages = pageSize > 0 ? (int)Math.Ceiling((double)total / pageSize) : 0;

        Collection = new CollectionMetadata
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

public class CollectionMetadata
{
    public string? Url { get; set; }
    public int Count { get; set; }
    public int Total { get; set; }
    public int Pages { get; set; }
    public string? Next { get; set; }
    public string? Prev { get; set; }
    public string? First { get; set; }
    public string? Last { get; set; }
}
