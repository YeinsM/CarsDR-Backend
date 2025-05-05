using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using CarSpot.Application.Interfaces;

namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MenuRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Menu> GetByIdAsync(Guid id)
        {
            return await _dbContext!.Menus
                .Include(c => c.Children)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Menu>> GetAllAsync()
        {
            return await _dbContext.Menus
                .Include(m => m.Children)
                .ToListAsync();
        }

        public async Task AddAsync(Menu menu)
        {
            await _dbContext.Menus.AddAsync(menu);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Menu menu)
        {
            _dbContext.Menus.Update(menu);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var menu = await GetByIdAsync(id);
            if (menu != null)
            {
                _dbContext.Menus.Remove(menu);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbContext.Menus.AnyAsync(m => m.Id == id);
        }
    }
}