using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CarSpot.Application.Interfaces
{
    public interface IExceptionMiddleware
    {
        Task InvokeAsync(HttpContext context);
    }
}
