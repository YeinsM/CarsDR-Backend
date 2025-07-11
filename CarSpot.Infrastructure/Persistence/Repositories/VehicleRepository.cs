using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;

        public VehicleRepository(ApplicationDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        public async Task<IEnumerable<VehicleDto>> GetAllAsync()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.MediaFiles)
                .Include(v => v.Make)
                .Include(v => v.Model)
                .Include(v => v.VehicleType)
                .Include(v => v.Color)
                .Include(v => v.Condition)
                .Include(v => v.Transmission)
                .Include(v => v.Drivetrain)
                .Include(v => v.CylinderOption)
                .Include(v => v.CabType)
                .Include(v => v.MarketVersion)
                .Include(v => v.VehicleVersion)
                .AsNoTracking()
                .ToListAsync();

            var result = vehicles.Select(v => new VehicleDto(
                v.Id,
                v.VIN,
                v.Price,
                v.Title!,
                v.IsFeatured,
                v.FeaturedUntil,
                v.Mileage,
                v.Year,
                v.VehicleType?.Name ?? "N/A",
                v.Make.Name,
                v.Model.Name!,
                v.Model.Id,
                v.Color.Name,
                v.Condition.Name,
                v.Transmission.Name,
                v.Drivetrain.Name,
                v.CylinderOption.Name,
                v.CabType.Name,
                v.MarketVersion.Name,
                v.VehicleVersion.Name,
                v.UserId,
               v.MediaFiles.Select(med => new VehicleMediaFileDto(
                med.Id,
                med.Url ?? ""
                )).ToList()
            ));

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
                .Include(v => v.MediaFiles)
                .Include(v => v.Comments)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            // Disparar el evento después de que el vehículo tenga un ID válido
            vehicle.NotifyVehicleCreated();
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

        public async Task DeleteByIdAsync(Guid id)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.MediaFiles)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle is null) return;

            foreach (var med in vehicle.MediaFiles)
            {
                if (med.ListingId != Guid.Empty)
                {
                    await _photoService.DeleteImageAsync(med.PublicId);
                }

                _context.VehicleMediaFiles.Remove(med);
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Vehicle>> FilterAsync(VehicleFilterRequest request)
        {
            var query = _context.Vehicles
                .Include(v => v.Make)
                .Include(v => v.Model)
                .Include(v => v.VehicleType)
                .Include(v => v.Condition)
                .Include(v => v.MarketVersion)
                .Include(v => v.City)
                .AsQueryable();

            if (request.VehicleTypeId.HasValue)
                query = query.Where(v => v.VehicleTypeId == request.VehicleTypeId);

            if (request.MakeId.HasValue)
                query = query.Where(v => v.MakeId == request.MakeId);

            if (request.ModelId.HasValue)
                query = query.Where(v => v.ModelId == request.ModelId);

            if (request.ConditionId.HasValue)
                query = query.Where(v => v.ConditionId == request.ConditionId);

            if (request.VehicleVersionId.HasValue)
                query = query.Where(v => v.MarketVersionId == request.VehicleVersionId);

            if (request.CityId.HasValue)
                query = query.Where(v => v.CityId == request.CityId);

            return await query.ToListAsync();
        }

    }
}
