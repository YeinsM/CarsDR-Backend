public class ListingResponse
{
    public Guid Id { get; set; }
   
    public required decimal Price { get; set; }
    public string? Currency { get; set; }
 
    public List<string>? Images { get; set; }
    public DateTime CreatedAt { get; set; }
}
