using CarSpot.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarSpot.Application.Interfaces
{
    public interface IListingStatusRepository
    {
        Task<IEnumerable<ListingStatus>> GetAllAsync();
    }
}
