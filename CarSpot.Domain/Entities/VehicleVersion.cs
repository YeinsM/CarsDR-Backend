using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;

public class VehicleVersion : BaseAuxiliar
{ 
    public int ModelId { get; private set; }
    public Model Model { get; private set; }=null!;
}
