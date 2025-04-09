using CarSpot.Domain.Common;

namespace CarSpot.Domain.Entities
{
    public class Model : BaseEntity
    {
        public string Name { get; private set; } = null!;
        public int MakeId { get; private set; }
        public Make? Make { get; private set; }
        public ICollection<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();

        public Model(string name, int makeId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Model name is required.");

            MakeId = makeId;
            Name = name;
        }


        public void Update(string name, int makeId)
        {
            Name = name;
            MakeId = makeId;
        }

    }
}
