using CarSpot.Domain.Entities;

public class VehicleMediaFile
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public string Url { get; set; } = default!;
    public string PublicId { get; set; } = default!;
    public string MediaType { get; set; } = default!;
    public Guid ListingId { get; set; } = default!;
    public Listing Listing { get; set; } = default!;
    public Vehicle Vehicle { get; set; } = default!;
}
