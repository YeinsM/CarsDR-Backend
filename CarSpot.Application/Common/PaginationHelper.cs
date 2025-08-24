using Microsoft.AspNetCore.Http;

namespace CarSpot.Application.Common;

public static class PaginationHelper
{
    public const int DefaultMaxPageSize = 100;
    
    public static (int pageNumber, int pageSize) ValidateParameters(PaginationParameters pagination, int maxPageSize = DefaultMaxPageSize)
    {
        int pageSize = pagination.PageSize > maxPageSize ? maxPageSize : pagination.PageSize;
        int pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;
        
        return (pageNumber, pageSize);
    }
    
    public static string BuildBaseUrl(HttpRequest request)
    {
        return $"{request.Scheme}://{request.Host}{request.Path}";
    }
}
