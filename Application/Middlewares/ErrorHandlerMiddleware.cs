using System.Net;
using System.Text.Json;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shared.Constants.Metadata;
using Shared.DTOs.Responses.Generals;
using Shared.Enums.Metadata;
using Shared.Enums.Time;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Metadata;

namespace Application.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            await HandleExceptionAsync(context, error);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception error)
    {
        HttpResponse response = context.Response;
        
        IContentTypeProvider contentTypeProvider = context.RequestServices.GetRequiredService<IContentTypeProvider>();
        IDateTimeHelper dateTimeHelper = context.RequestServices.GetRequiredService<IDateTimeHelper>();

        response.ContentType = contentTypeProvider.GetValue(ContentType.ApplicationJson, ContentTypeConstant.Json);
        response.StatusCode = GetStatusCode(error);
        
        string? httpMethod = context.Request.Method;
        string fullRequestUri = GetUri(context);
        
        var (offendingFile, offendingLine) = ExtractFileAndLineFromStackTrace(error.StackTrace);
        
        ErrorApiResponseDto responseModel = CreateResponse(error, response.StatusCode, httpMethod, dateTimeHelper, fullRequestUri, offendingFile, offendingLine);
        
        string result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });
        
        await response.WriteAsync(result);
    }
    private ErrorApiResponseDto CreateResponse(
        Exception exception,
        int statusCode,
        string httpMethod,
        IDateTimeHelper dateTimeHelper,
        string fullRequestUri,
        string? offendingFile,
        string? offendingLine)
    {
        Dictionary<string, List<string>> errors = exception is ValidationException validationException
            ? validationException.Errors.ToDictionary(k => k.Key, v => v.Value.ToList())
            : new Dictionary<string, List<string>> { { "error", new List<string> { exception.Message } } };

        return new ErrorApiResponseDto
        {
            StatusCode = statusCode,
            Message = exception.Message,
            httpMethod = httpMethod,
            Timestamp = dateTimeHelper.FormatDate(dateTimeHelper.GetCurrentUtcDateTime(), DateFormat.ShortDate),
            Errors = errors,
            additionalDetails = new AdditionalDetailsDto
            {
                FullRequestUri = fullRequestUri,
                File = offendingFile,
                Line = offendingLine
            }
        };
    }

    private string GetUri(HttpContext context)
    {
        return $"{context.Request.Path}{context.Request.QueryString}";
    }

    private int GetStatusCode(Exception error)
    {
        return error switch
        {
            ApiExcepcion _ => (int)HttpStatusCode.BadRequest,
            ValidationException _ => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException _ => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };
    }

    private (string? File, string? Line) ExtractFileAndLineFromStackTrace(string? stackTrace)
    {
        if (string.IsNullOrEmpty(stackTrace)) return (null, null);
        
        string? firstRelevantLine = stackTrace.Split('\n').FirstOrDefault(line => line.Contains(".cs"));
        string[]? fileInfo = firstRelevantLine?.Split(':');

        return fileInfo != null && fileInfo.Length >= 2
            ? (fileInfo[0].Trim(), fileInfo[1].Trim())
            : (null, null);
    }
}