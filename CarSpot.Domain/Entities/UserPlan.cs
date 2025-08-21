using CarSpot.Domain.Common;


namespace CarSpot.Domain.Entities
{
    public class UserPlan : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid PlanId { get; set; }
        public Plan Plan { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
