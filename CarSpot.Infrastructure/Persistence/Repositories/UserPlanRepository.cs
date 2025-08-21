
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Repositories
{
    public class UserPlanRepository : IUserPlanRepository
    {
        private readonly ApplicationDbContext _context;

        public UserPlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserPlan?> GetActivePlanByUserIdAsync(Guid userId)
        {
            return await _context.UserPlans
                .Where(p => p.UserId == userId 
                         && p.IsActive 
                         && p.StartDate <= DateTime.UtcNow 
                         && p.EndDate >= DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }
    }
}
