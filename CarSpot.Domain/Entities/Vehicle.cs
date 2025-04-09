using CarSpot.Domain.Common;

namespace CarSpot.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        public string VIN { get; private set; } = string.Empty;
        public int ModelId { get; private set; }
        public Model? Model { get; private set; }
        public int Year { get; private set; }
        public string Color { get; private set; } = string.Empty;



        public Vehicle(string vin, int year, int modelId, string color)
        {
            if (string.IsNullOrWhiteSpace(vin))
                throw new ArgumentNullException(nameof(vin), "VIN is required.");

            if (year < 1900 || year > DateTime.UtcNow.Year + 1)
                throw new ArgumentOutOfRangeException(nameof(year), "Year is invalid.");

            if (string.IsNullOrWhiteSpace(color))
                throw new ArgumentNullException(nameof(color), "Color is required.");

            VIN = vin;
            Year = year;
            ModelId = modelId;
            Color = color;
        }

        private Vehicle() { }

    }
        
}
