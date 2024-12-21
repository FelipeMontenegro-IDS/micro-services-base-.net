using Application.Wrappers.Bases;

namespace Application.Wrappers.Responses;

public class Response<T> : BaseResponse<T>
{
    public Response()
    {
    }
    
    public Response(T? data)
    {
        Data = data;
    }

    public Response(T? data, string message = null!)
    {
        Data = data;
        Message = message;
    }

    public Response(string message)
    {
        Message = message;
    }

    public Response(string message, T? data)
    {
        Data = data;
        Message = message;
    }

}