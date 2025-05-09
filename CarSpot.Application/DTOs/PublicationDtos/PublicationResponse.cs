public class PublicationResponse
{
    public Guid Id { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public decimal Price { get; set; }
    public string? Currency { get; set; }
    public string? Place { get; set; }
    public string? Version { get; set; }
    public List<string>? Images { get; set; }
    public DateTime CreatedAt { get; set; }
}
