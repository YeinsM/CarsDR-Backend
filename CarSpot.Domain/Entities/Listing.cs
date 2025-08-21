using CarSpot.Domain.Common;

namespace CarSpot.Domain.Entities
{
    public class Listing : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public decimal? ListingPrice { get; set; }

        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; } = null!;

        public int ListingStatusId { get; set; }
        public ListingStatus ListingStatus { get; set; } = null!;
        public DateTime? ExpiresAt { get; set; }

        
        public bool IsFeatured { get; private set; }
        public DateTime? FeaturedFrom { get; private set; }
        public DateTime? FeaturedUntil { get; private set; }

       
        public bool IsHighlighted { get; private set; }
        public DateTime? HighlightFrom { get; private set; }
        public DateTime? HighlightUntil { get; private set; }

        public int ViewCount { get; set; }

        public ICollection<VehicleMediaFile> MediaFiles { get; set; } = new List<VehicleMediaFile>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public Listing() { }

        public Listing(Guid userId, Guid vehicleId, string title, string description, decimal price, Guid currencyId, int listingStatusId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            VehicleId = vehicleId;
            Title = title;
            Description = description;
            Price = price;
            CurrencyId = currencyId;
            ListingStatusId = listingStatusId;
            CreatedAt = DateTime.UtcNow;
        }

        
        public void MarkAsFeatured(DateTime startDate, DateTime endDate)
        {
            IsFeatured = true;
            FeaturedFrom = startDate;
            FeaturedUntil = endDate;
        }

        public void RemoveFeatured()
        {
            IsFeatured = false;
            FeaturedFrom = null;
            FeaturedUntil = null;
        }

        public bool IsCurrentlyFeatured()
        {
            if (!IsFeatured || !FeaturedFrom.HasValue || !FeaturedUntil.HasValue)
                return false;

            var now = DateTime.UtcNow;
            if (now > FeaturedUntil.Value)
            {
                RemoveFeatured();
                return false;
            }

            return now >= FeaturedFrom.Value && now <= FeaturedUntil.Value;
        }

        
        public void MarkAsHighlighted(DateTime startDate, DateTime endDate)
        {
            IsHighlighted = true;
            HighlightFrom = startDate;
            HighlightUntil = endDate;
        }

        public void RemoveHighlighted()
        {
            IsHighlighted = false;
            HighlightFrom = null;
            HighlightUntil = null;
        }

        public bool IsCurrentlyHighlighted()
        {
            if (!IsHighlighted || !HighlightFrom.HasValue || !HighlightUntil.HasValue)
                return false;

            var now = DateTime.UtcNow;
            if (now > HighlightUntil.Value)
            {
                RemoveHighlighted();
                return false;
            }

            return now >= HighlightFrom.Value && now <= HighlightUntil.Value;
        }
    }
}
