using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using System.Text.Json.Serialization;

public class Role : BaseEntity
{

public string? Description { get; set; }
 [JsonIgnore]
public ICollection<User>? Users { get; set; }
}