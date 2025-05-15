public class CreateListingRequest
{
    public Guid UserId { get; set; }
    
    
    
    public string? Currency { get; set; }
    
    
    public List<string> Images { get; set; } = new();
}
