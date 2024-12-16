using Application.Wrappers.common.responses;

namespace Application.Wrappers.responses;

public class Response<T> : BaseResponse
{
    public Response()
    {
    }

    public T? Data { get; set; }

    public Response(T? data)
    {
        Data = data;
    }

    public Response(T? data, string message = null!)
    {
        Data = data;
        Message = message;
        Succeeded = true;
    }

    public Response(string message)
    {
        Message = message;
        Succeeded = false;
    }

}