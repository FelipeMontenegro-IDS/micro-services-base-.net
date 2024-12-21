using Application.Wrappers.Bases;

namespace Application.Wrappers.Responses;

public class ApiResponse<T> : BaseResponse<T>
{
    public int StatusCode { get; set; }
    public bool Succeeded  { get; set; }  
    
    public ApiResponse()
    {
    }
    
    public ApiResponse(T? data)
    {
        Data = data;
    }

    public ApiResponse(T? data, string message = null!)
    {
        Data = data;
        Message = message;
        Succeeded = true;
    }

    public ApiResponse(string message)
    {
        Message = message;
        Succeeded = false;
    }

    public ApiResponse(int statusCode, string message, T? data)
    {
        StatusCode = statusCode;
        Data = data;
        Message = message;
        Succeeded = data != null;
    }
}