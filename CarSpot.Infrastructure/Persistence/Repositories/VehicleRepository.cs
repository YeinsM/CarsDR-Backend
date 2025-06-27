using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class VehicleRepository(ApplicationDbContext _context) : IVehicleRepository
    {
        public async Task<IEnumerable<VehicleDto>> GetAllAsync()
        {
            var result = await (
                from v in _context.Vehicles

                join mk in _context.Makes on v.MakeId equals mk.Id
                join mo in _context.Models on v.ModelId equals mo.Id
                join c in _context.Colors on v.ColorId equals c.Id
                join cond in _context.Conditions on v.ConditionId equals cond.Id
                join t in _context.Transmissions on v.TransmissionId equals t.Id
                join d in _context.Drivetrains on v.DrivetrainId equals d.Id
                join cy in _context.CylinderOptions on v.CylinderOptionId equals cy.Id
                join cab in _context.CabTypes on v.CabTypeId equals cab.Id
                join mv in _context.MarketVersions on v.MarketVersionId equals mv.Id
                join vv in _context.VehicleVersions on v.VehicleVersionId equals vv.Id
                join u in _context.Users on v.UserId equals u.Id

                select new VehicleDto(
                    v.Id,
                    v.VIN,
                    v.Year,
                    mk.Name,
                    mo.Name,
                    mo.Id,
                    c.Name,
                    cond.Name,
                    t.Name,
                    d.Name,
                    cy.Name,
                    cab.Name,
                    mv.Name,
                    vv.Name,
                    v.UserId
                )
            ).AsNoTracking().ToListAsync();

            return result;
        }


        public async Task<Vehicle?> GetByIdAsync(Guid id)
        {
            return await _context.Vehicles
                .Include(v => v.Model)
                .Include(v => v.Make)
                .Include(v => v.User)
                .Include(v => v.VehicleVersion)
                .Include(v => v.MarketVersion)
                .Include(v => v.Transmission)
                .Include(v => v.Drivetrain)
                .Include(v => v.CylinderOption)
                .Include(v => v.CabType)
                .Include(v => v.Condition)
                .Include(v => v.Color)
                .Include(v => v.Images)
                .Include(v => v.Comments)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.Id == id);

        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            return vehicle;
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
