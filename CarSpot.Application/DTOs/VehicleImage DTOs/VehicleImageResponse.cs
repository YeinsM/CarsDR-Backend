public record ListingDto(
    int Id,
    decimal Price,
    string Currency,
    List<string> Images
);
