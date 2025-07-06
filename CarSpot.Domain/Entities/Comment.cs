using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;

public class Comment : BaseEntity
{

    public Guid VehicleId { get; set; }
    public Guid UserId { get; set; }
    public string? Content { get; set; }
    public Vehicle? Vehicle { get; set; }
    public User? User { get; set; }
}
