using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarSpot.Application.Interfaces.Repositories;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Repositories
{
    public class ModelRepository : IModelRepository
    {
        private readonly ApplicationDbContext _context;

        public ModelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Model?> GetByIdAsync(Guid id)
        {
            return await _context.Models
                .Include(m => m.Make)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IReadOnlyList<Model>> GetAllAsync()
        {
            return await _context.Models
                .Include(m => m.Make)
                .ToListAsync();
        }

        public async Task Add(Model model)
        {
            await _context.Models.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, string name, Guid makeId)
        {
            var model = await _context.Models.FindAsync(id);
            if (model is null)
                throw new Exception("Model not found");

            model.Update(name, makeId);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(Guid id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model is null)
                throw new Exception("Model not found");

            _context.Models.Remove(model);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Models.AnyAsync(m => m.Id == id);
        }
    }
}
