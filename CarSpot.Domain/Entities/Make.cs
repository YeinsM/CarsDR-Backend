using CarSpot.Domain.Common;

namespace CarSpot.Domain.Entities
{
    public class Make : BaseEntity
    {
        public string Name { get; private set; }
        public ICollection<Model> Models { get; private set; } = new List<Model>();

        public Make(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Make name is required.");

            Name = name;
        }
    }
}
