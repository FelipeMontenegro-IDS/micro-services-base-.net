namespace Application.Wrappers.responses;

public class ErrorApiResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public Dictionary<string,List<string>> Errors { get; set; }
}