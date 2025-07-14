public class PaginatedResponse<T>
{
    public CollectionMetadata Collection { get; set; } = default!;
    public IEnumerable<T> Data { get; set; } = default!;
}

public class CollectionMetadata
{
    public string Url { get; set; } = default!;
    public int Count { get; set; }
    public int Pages { get; set; }
    public int Total { get; set; }
    public string? Next { get; set; }
    public string? Prev { get; set; }
    public string First { get; set; } = default!;
    public string Last { get; set; } = default!;
}
