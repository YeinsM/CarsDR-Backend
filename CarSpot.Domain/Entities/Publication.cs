using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class Publication : BaseAuxiliar
{
    public Guid UserId { get; private set; }
    public Guid MakeId { get; private set; }
    public Guid ModelId { get; private set; }
    public Guid ColorId { get; private set; }
    public DateTime CreatedAt {get; set;}
    
    public required decimal Price { get; set; }
    public required string Currency { get; set; } = "USD";
    public required string Place { get; set; }
    public required string Version { get; set; }

    public List<string>? Images { get; private set; } = new();

    
    public User User { get; set; }  = null!;
    public Make Make { get; set; }  = null!;
    public Model Model { get; set; }  = null!;
    public Color Color { get; set; }  = null!;

    
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
