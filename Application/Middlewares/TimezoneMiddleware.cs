using System.Net;
using System.Text.Json;
using Application.Interfaces.services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shared.Constants.Metadata;
using Shared.DTOs.Responses.Generals;
using Shared.Enums.Metadata;
using Shared.Enums.Time;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Metadata;

namespace Application.Middlewares;

public class TimezoneMiddleware
{
    private readonly RequestDelegate _next;

    public TimezoneMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITimezoneService timezoneService, IDateTimeHelper dateTimeHelper)
    {
        try
        {
            if (!context.Request.Headers.TryGetValue("Timezone", out var timezoneHeader))
            {
                throw new ArgumentException("Timezone header is required.");
            }

            var timezoneId = timezoneHeader.ToString();
            timezoneService.SetTimezone(timezoneId);


            await _next(context);
        }
        catch (Exception error)
        {
            await HandleExceptionAsync(context, error,dateTimeHelper);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception error, IDateTimeHelper dateTimeHelper)
    {
        var response = context.Response;

        IContentTypeProvider contentTypeProvider = context.RequestServices.GetRequiredService<IContentTypeProvider>();
        
        response.ContentType = contentTypeProvider.GetValue(ContentType.ApplicationJson, ContentTypeConstant.Json);
        response.StatusCode = GetStatusCode(error);

        string httpMethod = context.Request.Method;
        string fullRequestUri = $"{context.Request.Path}{context.Request.QueryString}";

        var (offendingFile, offendingLine) = ExtractFileAndLineFromStackTrace(error.StackTrace);

        var responseModel = new ErrorApiResponseDto
        {
            StatusCode = response.StatusCode,
            Message = error.Message,
            httpMethod = httpMethod,
            Timestamp = dateTimeHelper.FormatDate(dateTimeHelper.GetCurrentUtcDateTime(), DateFormat.ShortDate),
            Errors = new Dictionary<string, List<string>>
            {
                { "error", new List<string> { error.Message } }
            },
            additionalDetails = new AdditionalDetailsDto
            {
                FullRequestUri = fullRequestUri,
                File = offendingFile,
                Line = offendingLine
            }
        };

        string result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await response.WriteAsync(result);
    }

    private int GetStatusCode(Exception error)
    {
        return error switch
        {
            ArgumentException _ => (int)HttpStatusCode.BadRequest,
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