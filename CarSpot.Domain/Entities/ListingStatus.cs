namespace CarSpot.Domain.Entities;

    public class ListingStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

         public ListingStatus(string name)
        {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    
    }

