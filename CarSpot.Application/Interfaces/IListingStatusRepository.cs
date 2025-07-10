using CarSpot.Domain.Entities;

namespace CarSpot.Application.Interfaces
{
    public interface IListingStatusRepository
    {
        Task<IEnumerable<ListingStatus>> GetAllAsync();
    }
}
