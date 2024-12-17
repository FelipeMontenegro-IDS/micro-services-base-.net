namespace Application.Wrappers.common.responses;

public abstract class BaseResponse
{
    public bool Succeeded  { get; set; }   
    public string Message { get; set; }   
    public List<string> Errors { get; set; }   
}
