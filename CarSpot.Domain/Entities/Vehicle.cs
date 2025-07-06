using CarSpot.Domain.Common;

namespace CarSpot.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        public string VIN { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid MakeId { get; set; }
        public Make Make { get; set; } = null!;

        public Guid ModelId { get; set; }
        public Model Model { get; set; } = null!;

        public int VehicleVersionId { get; set; }
        public VehicleVersion VehicleVersion { get; set; } = null!;

        public int MarketVersionId { get; set; }
        public MarketVersion MarketVersion { get; set; } = null!;

        public int TransmissionId { get; set; }
        public Transmission Transmission { get; set; } = null!;

        public int DrivetrainId { get; set; }
        public Drivetrain Drivetrain { get; set; } = null!;

        public int CylinderOptionId { get; set; }
        public CylinderOption CylinderOption { get; set; } = null!;

        public int CabTypeId { get; set; }
        public CabType CabType { get; set; } = null!;

        public int ConditionId { get; set; }
        public Condition Condition { get; set; } = null!;

        public int ColorId { get; set; }
        public Color Color { get; set; } = null!;

        public int Year { get; set; }
        public int Mileage { get; set; }
        public decimal Price { get; set; }

        public string Title { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public DateTime FeaturedUntil { get; set; }
        public int ViewCount { get; set; }

        public ICollection<VehicleImage> Images { get; set; } = new List<VehicleImage>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public Vehicle() { }
        public Vehicle(
            string vin,
            Guid userId,
            Guid makeId,
            Guid modelId,
            int vehicleVersionId,
            int marketVersionId,
            int transmissionId,
            int drivetrainId,
            int cylinderOptionId,
            int cabTypeId,
            int conditionId,
            int colorId,
            int year,
            int mileage,
            decimal price,
            string title,
            bool isFeatured,
            DateTime featuredUntil,
            int viewCount,
            DateTime createdAt
        )
        {
            if (string.IsNullOrWhiteSpace(vin))
                throw new ArgumentNullException(nameof(vin), "VIN cannot be null or empty.");
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.", nameof(userId));
            if (year < 1900 || year > DateTime.UtcNow.Year + 1)
                throw new ArgumentOutOfRangeException(nameof(year), "Year is invalid.");
            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be positive.");

            VIN = vin;
            UserId = userId;
            MakeId = makeId;
            ModelId = modelId;
            VehicleVersionId = vehicleVersionId;
            MarketVersionId = marketVersionId;
            TransmissionId = transmissionId;
            DrivetrainId = drivetrainId;
            CylinderOptionId = cylinderOptionId;
            CabTypeId = cabTypeId;
            ConditionId = conditionId;
            ColorId = colorId;
            Year = year;
            Mileage = mileage;
            Price = price;
            Title = title;
            IsFeatured = isFeatured;
            FeaturedUntil = featuredUntil;
            ViewCount = viewCount;
            CreatedAt = createdAt;
        }

        public void UpdateVehicle(string title, int mileage, decimal price, bool isFeatured, DateTime featuredUntil)
        {
            Title = title;
            Mileage = mileage;
            Price = price;
            IsFeatured = isFeatured;
            FeaturedUntil = featuredUntil;
        }

        public void AddImage(VehicleImage image)
        {
            Images.Add(image);
        }

        public void AddComment(Comment comment)
        {
            Comments.Add(comment);
        }
    }
}
