using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles!.ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(Guid id)
        {
            return await _context.Roles!.FindAsync(id);
        }

        public async Task<Role> CreateAsync(Role role)
        {
            _context.Roles!.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Roles!.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _context.Roles!.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
