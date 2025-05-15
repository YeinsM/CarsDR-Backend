public record UpdateListingRequest
(
    Guid MakeId,
    Guid ModelId,
    Guid ColorId,
    decimal Price,
    List<string> Images,
    string Currency = "USD", 
    string Place = null!,     
    string Version = null!     
);
