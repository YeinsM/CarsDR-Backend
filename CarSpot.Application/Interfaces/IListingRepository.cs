using CarSpot.Domain.Entities;

public interface IListingRepository
{
    IQueryable<Listing> Query();              
    Task<Listing?> GetByIdAsync(Guid id);
    Task<Listing> Add(Listing listing);
    Task UpdateAsync(Listing listing);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Listing>> GetBySellerIdAsync(Guid sellerId);

}
