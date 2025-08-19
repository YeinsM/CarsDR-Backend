

namespace CarSpot.Application.Interfaces.Services
{
    public interface IPlanService
    {
        
        Task<bool> UserHasActivePlan(Guid userId);

        
        Task<DateTime?> GetUserPlanExpiration(Guid userId);

  
        Task<bool> CanHighlightPublication(Guid userId);
    }
}
