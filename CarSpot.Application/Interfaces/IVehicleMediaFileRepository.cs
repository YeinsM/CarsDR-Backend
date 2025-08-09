

using CarSpot.Domain.Common;

public interface IVehicleMediaFileRepository
{
    Task<VehicleMediaFile?> GetByIdAsync(Guid id);
    Task AddAsync(VehicleMediaFile media);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<VehicleMediaFile>> GetByVehicleIdAsync(Guid vehicleId);
    Task<PaginatedResponse<VehicleMediaFile>> GetByVehicleIdPagedAsync(Guid vehicleId, int page, int pageSize, string baseUrl);

    Task SaveChangesAsync();
}
