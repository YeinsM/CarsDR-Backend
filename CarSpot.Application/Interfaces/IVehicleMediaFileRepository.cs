


public interface IVehicleMediaFileRepository
{
    Task<VehicleMediaFile?> GetByIdAsync(Guid id);
    Task AddAsync(VehicleMediaFile media);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<VehicleMediaFile>> GetByVehicleIdAsync(Guid vehicleId);
     IQueryable<VehicleMediaFile> Query();
   

    Task SaveChangesAsync();
}
