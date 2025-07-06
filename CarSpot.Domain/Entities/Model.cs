using System.Text.Json.Serialization;
using CarSpot.Domain.Common;

namespace CarSpot.Domain.Entities

{
    public class Model
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid MakeId { get; set; }
        public Make? Make { get; set; }
        [JsonIgnore]
        public ICollection<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();
        public ICollection<VehicleVersion> VehicleVersions { get; private set; } = new List<VehicleVersion>();

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
