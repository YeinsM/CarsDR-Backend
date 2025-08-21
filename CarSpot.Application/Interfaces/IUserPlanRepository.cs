using CarSpot.Domain.Entities;

public interface IUserPlanRepository
{
    Task<UserPlan?> GetActivePlanByUserIdAsync(Guid userId);
}
