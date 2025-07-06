namespace CarSpot.Application.Interfaces
{
    public interface IVehicleImageRepository
    {
        Task<IEnumerable<VehicleImage>> GetAllAsync();
        Task<VehicleImage?> GetByIdAsync(Guid id);
        Task<IEnumerable<VehicleImage>> GetByVehicleIdAsync(Guid id);
        Task<VehicleImage> CreateAsync(VehicleImage vehicleImage);
        Task<VehicleImage> UpdateAsync(VehicleImage vehicleImage);
        Task<VehicleImage> DeleteAsync(Guid id);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
