
using CarSpot.Domain.Common;

namespace CarSpot.Application.Interfaces.Services
{
    public interface IPaginationService
    {
        Task<PaginatedResponse<T>> PaginateAsync<T>(IQueryable<T> query, int pageNumber, int pageSize, string baseUrl);
    }
}
