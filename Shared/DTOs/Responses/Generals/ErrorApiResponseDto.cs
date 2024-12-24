namespace Shared.DTOs.Responses.Generals;

public class ErrorApiResponseDto
{
    public int StatusCode { get; set; }
    public string httpMethod { get; set; }

    public string Timestamp { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public Dictionary<string,List<string>> Errors { get; set; }
    public AdditionalDetailsDto additionalDetails { get; set; }
}

public class AdditionalDetailsDto
{
    public string FullRequestUri { get; set; }
    public string? File { get; set; }
    public string? Line { get; set; }
}