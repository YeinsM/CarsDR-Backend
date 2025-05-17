using CarSpot.Domain.Common;
using System.Text.Json.Serialization;

namespace CarSpot.Domain.Entities

{
    public class Model : BaseAuxiliar
    {
        
        public Guid MakeId { get; private set; }
        public Make? Make { get; private set; }
        [JsonIgnore]
        public ICollection<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();
        public ICollection<VehicleVersion> VehicleVersions {get; private set;} = new List<VehicleVersion>();

        public Model()
        {
        }

        public Model(string name, Guid makeId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Model name is required.");

            MakeId = makeId;
            Name = name;
        }


        public void Update(string name, Guid makeId)
        {
            Name = name;
            MakeId = makeId;
        }

    }
}
