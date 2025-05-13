public class PublicationResponse
{
    public Guid Id { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public required string Color { get; set; }
    public required decimal Price { get; set; }
    public string? Currency { get; set; }
    public string? Place { get; set; }
    public string? Version { get; set; }
    public List<string>? Images { get; set; }
    public DateTime CreatedAt { get; set; }
}
