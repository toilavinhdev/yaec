using System.Net;

namespace Package.Shared.ValueObjects;

public class ApiResponse
{
    public int Code { get; set; }
    
    public string? Message { get; set; }

    public ApiResponse(string? message = null)
    {
        Code = (int)HttpStatusCode.OK;
        Message = message;
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
    
    public ApiResponse(T? data, string? message = null) : base(message)
    {
        Data = data;
    }
}