using System.Linq;
using System.Threading.Tasks;
using CarSpot.Application.Common;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces.Services;
using CarSpot.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace CarSpot.WebApi.Controllers.Base;

[ApiController]
public abstract class PaginatedControllerBase : ControllerBase
{
    private readonly IPaginationService _paginationService;

    protected PaginatedControllerBase(IPaginationService paginationService)
    {
        _paginationService = paginationService;
    }

    /// <summary>
    /// Método base para manejar paginación de manera consistente
    /// </summary>
    /// <typeparam name="T">Tipo de dato a paginar</typeparam>
    /// <param name="query">Query a paginar</param>
    /// <param name="pagination">Parámetros de paginación</param>
    /// <param name="message">Mensaje de éxito opcional</param>
    /// <param name="useApiResponseBuilder">Si debe envolver la respuesta con ApiResponseBuilder</param>
    /// <returns>Respuesta paginada</returns>
    protected async Task<ActionResult<PaginatedResponse<T>>> GetPaginatedResultAsync<T>(
        IQueryable<T> query,
        PaginationParameters pagination,
        string? message = null,
        bool useApiResponseBuilder = false)
    {
        var (pageNumber, pageSize) = PaginationHelper.ValidateParameters(pagination);
        string baseUrl = PaginationHelper.BuildBaseUrl(Request);

        var paginatedResult = await _paginationService.PaginateAsync(
            query,
            pageNumber,
            pageSize,
            baseUrl
        );

        return useApiResponseBuilder && !string.IsNullOrEmpty(message)
            ? Ok(ApiResponseBuilder.Success(paginatedResult, message))
            : Ok(paginatedResult);
    }
}
