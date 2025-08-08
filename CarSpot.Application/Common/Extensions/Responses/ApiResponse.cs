namespace CarSpot.Application.Common.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public object? Metadata { get; set; }

        public ApiResponse() { }

        public ApiResponse(T data, object? metadata = null)
        {
            Data = data;
            Metadata = metadata;
        }
    }
}
