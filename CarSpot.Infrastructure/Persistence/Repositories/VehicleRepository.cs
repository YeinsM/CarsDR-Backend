using CarSpot.Application.Common.Helpers;
using CarSpot.Domain.Common;
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
                v.IsFeatured ?? false,
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

        public async Task<PaginatedResponse<Vehicle>> FilterAsync(VehicleFilterRequest request)
        {
            IQueryable<Vehicle> query = _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.Make)
                .Include(v => v.Model)
                .Include(v => v.Condition)
                .Include(v => v.VehicleVersion)
                .Include(v => v.City);

            
            if (!string.IsNullOrWhiteSpace(request.VehicleType))
            {
                string value = request.VehicleType.ToLower();
                query = query.Where(v =>
                    v.VehicleType.Name != null &&
                    v.VehicleType.Name.ToLower().Contains(value));
            }

            if (!string.IsNullOrWhiteSpace(request.Make))
            {
                string value = request.Make.ToLower();
                query = query.Where(v =>
                    v.Make.Name != null &&
                    v.Make.Name.ToLower().Contains(value));
            }

            if (!string.IsNullOrWhiteSpace(request.Model))
            {
                string value = request.Model.ToLower();
                query = query.Where(v =>
                    v.Model.Name != null &&
                    v.Model.Name.ToLower().Contains(value));
            }

            if (!string.IsNullOrWhiteSpace(request.Condition))
            {
                string value = request.Condition.ToLower();
                query = query.Where(v =>
                    v.Condition.Name != null &&
                    v.Condition.Name.ToLower().Contains(value));
            }

            if (!string.IsNullOrWhiteSpace(request.Version))
            {
                string value = request.Version.ToLower();
                query = query.Where(v =>
                    v.VehicleVersion.Name != null &&
                    v.VehicleVersion.Name.ToLower().Contains(value));
            }

            if (!string.IsNullOrWhiteSpace(request.City))
            {
                string value = request.City.ToLower();
                query = query.Where(v =>
                    v.City.Name != null &&
                    v.City.Name.ToLower().Contains(value));
            }

            
            int page = request.Page <= 0 ? 1 : request.Page;
            int pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            return await PaginationHelper.CreatePaginatedResponse(
                query,
                page,
                pageSize,
                "/api/vehicles/filter",
                request.OrderBy,
                request.SortDir
            );
        }




        public IQueryable<Vehicle> Query()
        {
            return _context.Vehicles.AsQueryable();
        }



    }
}
