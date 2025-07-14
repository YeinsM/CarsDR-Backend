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
                .Include(v => v.VehicleType)
                .Include(v => v.Make)
                .Include(v => v.Model)
                .Include(v => v.Condition)
                .Include(v => v.City)
                .Include(v => v.VehicleVersion)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.VehicleType))
                query = query.Where(v =>
                    v.VehicleType != null &&
                    v.VehicleType.Name.ToLower() == request.VehicleType.ToLower());

            if (!string.IsNullOrWhiteSpace(request.Make))
                query = query.Where(v =>
                    v.Make != null &&
                    v.Make.Name.ToLower() == request.Make.ToLower());

            if (!string.IsNullOrWhiteSpace(request.Model))
                query = query.Where(v =>
                    v.Model != null &&
                    v.Model.Name.ToLower() == request.Model.ToLower());

            if (!string.IsNullOrWhiteSpace(request.Condition))
                query = query.Where(v =>
                    v.Condition != null &&
                    v.Condition.Name.ToLower() == request.Condition.ToLower());

            if (!string.IsNullOrWhiteSpace(request.Version))
                query = query.Where(v =>
                    v.VehicleVersion != null &&
                    v.VehicleVersion.Name.ToLower() == request.Version.ToLower());

            if (!string.IsNullOrWhiteSpace(request.City))
                query = query.Where(v =>
                    v.City != null &&
                    v.City.Name.ToLower() == request.City.ToLower());

            return await query.ToListAsync();
        }

        public IQueryable<Vehicle> Query()
        {
            return _context.Vehicles.AsQueryable();
        }



    }
}
