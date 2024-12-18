namespace Application.Wrappers.responses;

public class ErrorApiResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public Dictionary<string,List<string>> Errors { get; set; }
    public AdditionalDetails additionalDetails { get; set; }
}

public class AdditionalDetails
{
    public string FullRequestUri { get; set; }
    public string? File { get; set; }
    public string? Line { get; set; }
}