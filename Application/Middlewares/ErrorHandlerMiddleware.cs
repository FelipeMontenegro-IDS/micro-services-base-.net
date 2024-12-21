using System.Net;
using System.Text.Json;
using Application.Exceptions;
using Application.Wrappers.Responses;
using Microsoft.AspNetCore.Http;

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
            var response = context.Response;
            response.ContentType = "application/json";

            var httpMethod = context.Request.Method;
            var responseModel = new ErrorApiResponse { Message = error.Message };


            var fullRequestUri = GetUri(context);
            var (offendingFile, offendingLine) = ExtractFileAndLineFromStackTrace(error.StackTrace);

            switch (error)
            {
                case ApiExcepcion e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel = CreateResponse(
                        e, (int)HttpStatusCode.BadRequest,
                        httpMethod,
                        fullRequestUri,
                        offendingFile,
                        offendingLine);
                    break;
                case Application.Exceptions.ValidationException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel = CreateResponse(
                        e,
                        (int)HttpStatusCode.BadRequest,
                        httpMethod,
                        fullRequestUri,
                        offendingFile,
                        offendingLine,
                        e.Errors.ToDictionary(
                            k => k.Key,
                            v => v.Value.ToList()
                        )
                    );
                    break;
                case KeyNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    responseModel = CreateResponse
                    (e,
                        (int)HttpStatusCode.NotFound,
                        httpMethod,
                        fullRequestUri,
                        offendingFile,
                        offendingLine
                    );
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseModel = CreateResponse
                    (
                        error,
                        (int)HttpStatusCode.InternalServerError,
                        httpMethod,
                        fullRequestUri,
                        offendingFile,
                        offendingLine
                    );
                    break;
            }

            var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            await response.WriteAsync(result);
        }
    }

    private ErrorApiResponse CreateResponse(
        Exception exception,
        int statusCode,
        string httpMethod,
        string fullRequestUri,
        string? offendingFile,
        string? offendingLine,
        Dictionary<string, List<string>> errors = null!)
    {
        return new ErrorApiResponse
        {
            StatusCode = statusCode,
            Message = exception.Message,
            httpMethod = httpMethod,
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Errors = exception is Application.Exceptions.ValidationException
                ? errors
                : new Dictionary<string, List<string>>
                {
                    { "error", new List<string> { exception.Message } }
                },
            additionalDetails = new AdditionalDetails()
            {
                FullRequestUri = fullRequestUri,
                File = offendingFile,
                Line = offendingLine
            }
        };
    }

    private string GetUri(HttpContext context)
    {
        //{context.Request.Scheme}://{context.Request.Host}
        return $"{context.Request.Path}{context.Request.QueryString}";
    }

    private (string? File, string? Line) ExtractFileAndLineFromStackTrace(string? stackTrace)
    {
        if (string.IsNullOrEmpty(stackTrace))
        {
            return (null, null);
        }

        var stackLines = stackTrace.Split('\n'); // Dividir por líneas el StackTrace
        var firstRelevantLine = stackLines.FirstOrDefault(line => line.Contains(".cs"));
        var fileInfo = firstRelevantLine?.Split(':');

        if (fileInfo != null && fileInfo.Length >= 2)
        {
            var file = fileInfo[0]?.Trim(); // Archivo donde ocurrió la excepción
            var line = fileInfo[1]?.Trim(); // Línea de código
            return (file, line);
        }

        return (null, null);
    }
}