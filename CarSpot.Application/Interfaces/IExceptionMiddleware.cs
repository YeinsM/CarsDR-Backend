using Microsoft.AspNetCore.Http;

namespace CarSpot.Infrastructure.Middleware
{
    public interface IExceptionMiddleware
    {
        Task InvokeAsync(HttpContext context, RequestDelegate next);
    }
}
