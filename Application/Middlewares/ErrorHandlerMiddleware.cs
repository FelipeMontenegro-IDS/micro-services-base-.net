using System.Net;
using System.Text.Json;
using Application.Exceptions;
using Application.Wrappers.responses;
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
            var responseModel = new ErrorApiResponse { Message = error.Message };

            switch (error)
            {
                case ApiExcepcion e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel = CreateResponse(e, (int)HttpStatusCode.BadRequest);
                    break;
                case Application.Exceptions.ValidationException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel = CreateResponse(
                        e,
                        (int)HttpStatusCode.BadRequest,
                        e.Errors.ToDictionary(
                            k => k.Key,
                            v => v.Value.ToList()
                        )
                    );
                    break;
                case KeyNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    responseModel = CreateResponse(e, (int)HttpStatusCode.NotFound);
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseModel = CreateResponse(error, (int)HttpStatusCode.InternalServerError);
                    break;
            }

            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
    }

    private ErrorApiResponse CreateResponse(Exception exception, int statusCode,
        Dictionary<string, List<string>> errors = null!)
    {
        return new ErrorApiResponse
        {
            StatusCode = statusCode,
            Message = exception.Message,
            Errors = exception is Application.Exceptions.ValidationException
                ? errors
                : new Dictionary<string, List<string>>
                {
                    { "error", new List<string> { exception.Message } }
                }
        };
    }
}