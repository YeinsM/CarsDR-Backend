
using Microsoft.AspNetCore.Http;



namespace CarSpot.Application.Common.Responses
{
    public static class ApiResponseBuilder
    {
        public static ApiResponse<T> Success<T>(T? data, string? message = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message ?? "Successful operation.",
                Data = data,
                StatusCode = StatusCodes.Status200OK
            };
        }

        public static ApiResponse<T> Fail<T>(int statusCode, string message, T? data = default)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }
    }
}
