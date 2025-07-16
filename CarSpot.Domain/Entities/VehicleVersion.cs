using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;

public class VehicleVersion : BaseAuxiliar
{
    public Guid ModelId { get; set; }
    public Model Model { get; set; } = null!;
}
