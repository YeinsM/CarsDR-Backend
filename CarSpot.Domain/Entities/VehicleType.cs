using CarSpot.Domain.Common;

namespace CarSpot.Domain.Entities
{
    public class VehicleType : BaseAuxiliar
    {
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public const string Carro = "Carro";
    }
}
