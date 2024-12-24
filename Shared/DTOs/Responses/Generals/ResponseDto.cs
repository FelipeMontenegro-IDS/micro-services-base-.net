using Shared.Bases;

namespace Shared.DTOs.Responses.Generals;

public class ResponseDto<T> : BaseResponse<T>
{
    public ResponseDto()
    {
    }
    
    public ResponseDto(T? data)
    {
        Data = data;
    }

    public ResponseDto(T? data, string message = null!)
    {
        Data = data;
        Message = message;
    }

    public ResponseDto(string message)
    {
        Message = message;
    }

    public ResponseDto(string message, T? data)
    {
        Data = data;
        Message = message;
    }
}