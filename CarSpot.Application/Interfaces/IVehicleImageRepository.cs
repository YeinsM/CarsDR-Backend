public interface IVehicleImageRepository
{
    Task<IEnumerable<VehicleImage>> GetAllAsync();
    Task<VehicleImage?> GetByIdAsync(Guid id);
    Task<IEnumerable<VehicleImage>> GetByVehicleIdAsync(Guid vehicleId);
    Task AddAsync(VehicleImage image);
    Task DeleteAsync(Guid id);
}
