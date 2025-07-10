namespace CarSpot.Application.DTOS
{
    public class ListingResponse
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid VehicleId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public Guid CurrencyId { get; set; }

        public int ListingStatusId { get; set; }

        public bool IsFeatured { get; set; }
        public DateTime? FeaturedUntil { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public int ViewCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<string> ImageUrls { get; set; } = new();
    }
}
