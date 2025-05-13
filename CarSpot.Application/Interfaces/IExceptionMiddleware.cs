using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CarSpot.Application.Interfaces
{
    public interface IExceptionMiddleware
    {
        Task InvokeAsync(HttpContext context);
    }
}
