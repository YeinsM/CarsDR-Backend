using System;
using System.Collections.Generic;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;

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

        public bool IsFeatured { get; set; }
        public DateTime? FeaturedUntil { get; set; }

        public int ViewCount { get; set; }

        public ICollection<VehicleImage> Images { get; set; } = new List<VehicleImage>();

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
    }
}
