namespace Application.Wrappers.responses;

public abstract class BaseResponse
{
    public bool Succeeded  { get; set; }   
    public string Message { get; set; }   
}
