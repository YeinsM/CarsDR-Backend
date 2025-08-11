using System.Text.Json.Serialization;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;

public class Role : BaseEntity
{

    public string Description { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<User>? Users { get; set; }
}
