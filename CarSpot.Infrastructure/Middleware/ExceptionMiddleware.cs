
using System.Text.Json;
using CarSpot.Application.Common.Responses;
using CarSpot.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CarSpot.Infrastructure.Middleware
{
    public class ExceptionMiddleware : IExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.HasStarted)
                    return;

                var statusCode = context.Response.StatusCode;

                switch (statusCode)
                {
                    case StatusCodes.Status405MethodNotAllowed:
                        await WriteFormattedResponse(context, statusCode, "Método no permitido.");
                        break;

                    case StatusCodes.Status429TooManyRequests:
                        await WriteFormattedResponse(context, statusCode, "Demasiadas peticiones. Intenta más tarde.");
                        break;

                    case StatusCodes.Status503ServiceUnavailable:
                        await WriteFormattedResponse(context, statusCode, "Servicio no disponible temporalmente.");
                        break;

                    case StatusCodes.Status204NoContent:
                        await WriteFormattedResponse(context, statusCode, "Sin contenido.");
                        break;

                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción no controlada");

                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var errorResponse = ApiResponseBuilder.Fail(
                        StatusCodes.Status500InternalServerError,
                        "Error interno del servidor",
                        $"{ex.Message} - {ex.Source}"
                    );

                    var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    await context.Response.WriteAsync(json);
                }
            }
        }

        private async Task WriteFormattedResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";

            ApiResponse<object?> response;

            if (statusCode == StatusCodes.Status204NoContent)
            {
                response = ApiResponseBuilder.Success<object?>(null, message);
            }
            else
            {
                response = ApiResponseBuilder.Fail<object?>(statusCode, message);
            }

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }

    }
}
