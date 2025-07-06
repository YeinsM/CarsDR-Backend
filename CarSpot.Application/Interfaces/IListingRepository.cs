using CarSpot.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarSpot.Application.Interfaces
{
    public interface IListingRepository
    {
        Task<IEnumerable<Listing>> GetAllAsync();
        Task<Listing?> GetByIdAsync(Guid id);
        Task<Listing> Add(Listing listing);
        Task<Listing> UpdateAsync(Listing listing);
        Task<Listing> DeleteAsync(Guid id);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
