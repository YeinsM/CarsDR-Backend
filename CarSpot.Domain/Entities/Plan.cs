using CarSpot.Domain.Common;

namespace CarSpot.Domain.Entities
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }

        public ICollection<UserPlan> UserPlans { get; set; } = new List<UserPlan>();
    }
}
