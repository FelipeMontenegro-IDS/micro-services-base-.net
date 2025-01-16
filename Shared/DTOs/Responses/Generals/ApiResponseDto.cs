using Shared.Bases.Responses;

namespace Shared.DTOs.Responses.Generals;

public class ApiResponseDto<T> : BaseResponse<T>
{
    public int StatusCode { get; set; }
    public bool Succeeded  { get; set; }  
    
    public ApiResponseDto()
    {
    }
    
    public ApiResponseDto(T? data)
    {
        Data = data;
    }

    public ApiResponseDto(T? data, string message = null!)
    {
        Data = data;
        Message = message;
        Succeeded = true;
    }

    public ApiResponseDto(string message)
    {
        Message = message;
        Succeeded = false;
    }

    public ApiResponseDto(int statusCode, string message, T? data)
    {
        StatusCode = statusCode;
        Data = data;
        Message = message;
        Succeeded = data != null;
    }
}