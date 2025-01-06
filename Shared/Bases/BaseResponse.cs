namespace Shared.Bases;

public class BaseResponse<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }   
    public List<string> Errors { get; set; } 
}