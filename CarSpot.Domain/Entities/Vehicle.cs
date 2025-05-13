using System.ComponentModel.DataAnnotations;
using CarSpot.Domain.Common;

namespace CarSpot.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        public Vehicle() { }
        public required string VIN { get; set; }
        public Guid UserId { get; set; }
        public required User User { get; set; }
        
        public Guid MakeId { get; set; }
        public required Make Make { get; set; }

        public Guid ModelId { get; set; }
        public required Model Model { get; set; }

        public Guid? VersionId { get; set; }
        public Version? Version { get; set; }

        public Guid? MarketVersionId { get; set; }
        public MarketVersion? MarketVersion { get; set; }

        public Guid? TransmissionId { get; set; }
        public Transmission? Transmission { get; set; }

        public Guid? DrivetrainId { get; set; }
        public Drivetrain? Drivetrain { get; set; }

        public Guid? CylinderOptionId { get; set; }
        public CylinderOption? CylinderOption { get; set; }

        public Guid? CabTypeId { get; set; }
        public CabType? CabType { get; set; }

        public Guid ConditionId { get; set; }
        public required Condition Condition { get; set; }

        public Guid? ColorId { get; set; }
        public Color? Color { get; set; }

        public int Year { get; set; }

        public int? Mileage { get; set; }

        public decimal Price { get; set; }

        public string? Title { get; set; }

        public bool IsFeatured { get; set; }

        public DateTime? FeaturedUntil { get; set; }

        public int ViewCount { get; set; }

        public ICollection<VehicleImage>? Images { get; set; }
        public ICollection<Comment>? Comments { get; set; }

        
        public Vehicle(
            string vin,
            Guid userId, 
            Guid makeId, 
            Guid modelId, 
            int year, 
            decimal price, 
            Guid conditionId, 
            string? title = null,
            int? mileage = null,
            bool isFeatured = false,
            DateTime? featuredUntil = null,
            int viewCount = 0,
            DateTime? createdAt = null)
        {
            
            if (string.IsNullOrWhiteSpace(vin))
                throw new ArgumentNullException(nameof(vin), "VIN cannot be null or empty.");
            
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId), "UserId cannot be empty.");

            if (makeId == Guid.Empty)
                throw new ArgumentNullException(nameof(makeId), "MakeId cannot be empty.");

            if (modelId == Guid.Empty)
                throw new ArgumentNullException(nameof(modelId), "ModelId cannot be empty.");

            if (year < 1900 || year > DateTime.UtcNow.Year + 1)
                throw new ArgumentOutOfRangeException(nameof(year), "Year is invalid.");

            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be a positive number.");

            if (conditionId == Guid.Empty)
                throw new ArgumentNullException(nameof(conditionId), "ConditionId cannot be empty.");

        
            VIN = vin;
            UserId = userId;
            MakeId = makeId;
            ModelId = modelId;
            Year = year;
            Price = price;
            ConditionId = conditionId;
            Title = title;
            Mileage = mileage;
            IsFeatured = isFeatured;
            FeaturedUntil = featuredUntil;
            ViewCount = viewCount;
            CreatedAt = createdAt ?? DateTime.UtcNow;

            
            Images = new List<VehicleImage>();
            Comments = new List<Comment>();
        }

        
        public void UpdateVehicle(
            string? title,
            int? mileage,
            decimal price,
            bool isFeatured,
            DateTime? featuredUntil)
        {
            Title = title;
            Mileage = mileage;
            Price = price;
            IsFeatured = isFeatured;
            FeaturedUntil = featuredUntil;
        }

        
        public void AddImage(VehicleImage image)
        {
            if (Images == null)
                Images = new List<VehicleImage>();

            Images.Add(image);
        }

        
        public void AddComment(Comment comment)
        {
            if (Comments == null)
                Comments = new List<Comment>();

            Comments.Add(comment);
        }

        
       
    }
}
