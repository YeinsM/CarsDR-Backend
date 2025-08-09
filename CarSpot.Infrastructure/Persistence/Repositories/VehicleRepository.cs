using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;


namespace CarSpot.Infrastructure.Persistence.Repositories
{
    public class VehicleRepository(ApplicationDbContext context, IPhotoService photoService) : IVehicleRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IPhotoService _photoService = photoService;

        public async Task<IEnumerable<VehicleDto>> GetAllAsync()
        {
            List<Vehicle> vehicles = await _context.Vehicles
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

            return vehicles.Select(v => new VehicleDto(
                v.Id,
                v.VIN,
                v.Price,
                v.Title ?? "",
                v.IsFeatured ?? false,
                v.FeaturedUntil,
                v.Mileage,
                v.Year,
                v.VehicleType?.Name ?? "N/A",
                v.Make.Name,
                v.Model.Name ?? "",
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
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

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

        public async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<Vehicle> Query()
        {
            return _context.Vehicles
                .Include(v => v.Make)
                .Include(v => v.Model)
                .Include(v => v.Condition)
                .Include(v => v.Drivetrain)
                .Include(v => v.CylinderOption)
                .Include(v => v.CabType)
                .Include(v => v.MediaFiles)
                .AsNoTracking();
        }


        public async Task<PaginatedResponse<VehicleDto>> FilterAsync(VehicleFilterRequest filter, string baseUrl)
        {
            IQueryable<Vehicle> query = _context.Vehicles
                .Include(v => v.Make)
                .Include(v => v.Model)
                .Include(v => v.Condition)
                .Include(v => v.Drivetrain)
                .Include(v => v.CylinderOption)
                .Include(v => v.CabType)
                .AsQueryable();

            if (filter.MakeId.HasValue)
                query = query.Where(v => v.MakeId == filter.MakeId.Value);

            if (filter.ModelId.HasValue)
                query = query.Where(v => v.ModelId == filter.ModelId.Value);

            if (filter.ConditionId.HasValue)
                query = query.Where(v => v.ConditionId == filter.ConditionId.Value);

            if (filter.DrivetrainId.HasValue)
                query = query.Where(v => v.DrivetrainId == filter.DrivetrainId.Value);

            if (filter.CylinderOptionId.HasValue)
                query = query.Where(v => v.CylinderOptionId == filter.CylinderOptionId.Value);

            if (filter.CabTypeId.HasValue)
                query = query.Where(v => v.CabTypeId == filter.CabTypeId.Value);

            if (filter.MinMileage.HasValue && filter.MaxMileage.HasValue)
            {
                if (filter.MinMileage > filter.MaxMileage)
                    throw new ArgumentException("MinMileage cannot be greater than MaxMileage.");

                query = query.Where(v => v.Mileage >= filter.MinMileage.Value && v.Mileage <= filter.MaxMileage.Value);
            }
            else if (filter.MinMileage.HasValue)
            {
                query = query.Where(v => v.Mileage >= filter.MinMileage.Value);
            }
            else if (filter.MaxMileage.HasValue)
            {
                query = query.Where(v => v.Mileage <= filter.MaxMileage.Value);
            }

            if (filter.MinYear.HasValue && filter.MaxYear.HasValue)
            {
                if (filter.MinYear > filter.MaxYear)
                    throw new ArgumentException("MinYear cannot be greater than MaxYear.");

                query = query.Where(v => v.Year >= filter.MinYear.Value && v.Year <= filter.MaxYear.Value);
            }
            else if (filter.MinYear.HasValue)
            {
                query = query.Where(v => v.Year >= filter.MinYear.Value);
            }
            else if (filter.MaxYear.HasValue)
            {
                query = query.Where(v => v.Year <= filter.MaxYear.Value);
            }

            int totalItems = await query.CountAsync();

            List<Vehicle> vehicles = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var vehicleDtos = vehicles.Select(v => new VehicleDto(
                v.Id,
                v.VIN,
                v.Price,
                v.Title ?? "",
                v.IsFeatured ?? false,
                v.FeaturedUntil,
                v.Mileage,
                v.Year,
                v.VehicleType?.Name ?? "N/A",
                v.Make.Name,
                v.Model.Name ?? "",
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

            return new PaginatedResponse<VehicleDto>(
                vehicleDtos,
                filter.Page,
                filter.PageSize,
                totalItems,
                baseUrl
            );
        }
    }
}
