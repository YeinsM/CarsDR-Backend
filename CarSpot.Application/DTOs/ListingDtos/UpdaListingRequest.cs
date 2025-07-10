namespace CarSpot.Application.DTOs
{
    public class UpdateListingRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? ListingPrice { get; set; }
        public Guid CurrencyId { get; set; }
        public int ListingStatusId { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? FeaturedUntil { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }

}
