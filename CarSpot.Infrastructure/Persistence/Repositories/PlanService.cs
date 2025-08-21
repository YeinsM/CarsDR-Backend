using CarSpot.Application.Interfaces.Services;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Services
{
    public class PlanService : IPlanService
    {
        private readonly ApplicationDbContext _context;

        public PlanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserHasActivePlan(Guid userId)
        {
            return await _context.UserPlans
                .AnyAsync(up => up.UserId == userId && up.EndDate > DateTime.UtcNow);
        }

        public async Task<DateTime?> GetUserPlanExpiration(Guid userId)
        {
            var plan = await _context.UserPlans
                .Where(up => up.UserId == userId && up.EndDate > DateTime.UtcNow)
                .OrderByDescending(up => up.EndDate)
                .FirstOrDefaultAsync();

            return plan?.EndDate;
        }

        public async Task<bool> CanHighlightPublication(Guid userId)
        {
           
            return await UserHasActivePlan(userId);
        }
    }
}
