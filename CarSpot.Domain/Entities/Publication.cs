using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class Publication : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid MakeId { get; private set; }
    public Guid ModelId { get; private set; }
    public Guid ColorId { get; private set; }
    
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = "USD";
    public string Place { get; private set; }
    public string Version { get; private set; }

    public List<string> Images { get; private set; } = new();

    
    public User User { get; private set; }
    public Make Make { get; private set; }
    public Model Model { get; private set; }
    public Color Color { get; private set; }

    
    public Publication(Guid userId, Guid makeId, Guid modelId, Guid colorId,
                       decimal price, string currency, string place, string version,
                       List<string> images)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        MakeId = makeId;
        ModelId = modelId;
        ColorId = colorId;
        Price = price;
        Currency = currency;
        Place = place;
        Version = version;
        Images = images ?? new List<string>();
        CreatedAt = DateTime.UtcNow;
    }


}
