using Microsoft.AspNetCore.Http;

namespace Identity.Api.Contracts.Dtos.Response
{


    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; } = 200; // Default to 200 OK
        public string? ErrorMsg { get; set; }

        public static ApiResponse<T> Ok(T data, int statusCode = 200)
        {
            var response = new ApiResponse<T>();

            response.Success = true;
            response.StatusCode = statusCode;
            response.Data = data;
            return response;
        }

        public static ApiResponse<T> Fail(string errMsg, int statusCode = 500, T? data = default)
        {
            var response = new ApiResponse<T>();

            response.Success = false;
            response.ErrorMsg = errMsg;
            response.StatusCode = statusCode;
            response.Data = data;
            return response;
        }
    }
}
