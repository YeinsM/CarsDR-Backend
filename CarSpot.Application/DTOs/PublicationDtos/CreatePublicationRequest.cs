public class CreatePublicationRequest
{
    public Guid UserId { get; set; }
    public Guid MakeId { get; set; }
    public Guid ModelId { get; set; }
    public Guid ColorId { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public string Place { get; set; }
    public string Version { get; set; }
    public List<string> Images { get; set; } = new();
}
