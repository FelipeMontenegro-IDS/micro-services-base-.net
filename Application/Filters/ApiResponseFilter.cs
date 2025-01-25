using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.DTOs.Responses.Generals;

namespace Application.Filters;

public class ApiResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult)
        {
            string? httpMethod = context.HttpContext.Request.Method; // Tipo de método HTTP (GET, POST, PUT, DELETE)
            Object? data = objectResult.Value;
            ApiResponseDto<object> apiResponse = CreateApiResponse(httpMethod, objectResult.StatusCode, data);

            context.Result = new ObjectResult(apiResponse) { StatusCode = objectResult.StatusCode };
        }
    }

    private ApiResponseDto<Object> CreateApiResponse(string httpMethod, int? statusCode, object data)
    {
        return httpMethod switch
        {
            var method when
                method == HttpMethod.Get.Method => HandleGetResponse(data),
            var method when
                method.Equals(HttpMethod.Post.ToString()) => CreateResponse(statusCode ?? StatusCodes.Status201Created,
                    "Resource created successfully.", data),
            var method when
                method.Equals(HttpMethod.Put.ToString()) => CreateResponse(statusCode ?? StatusCodes.Status200OK,
                    "Resource updated successfully.", data),
            var method when
                method.Equals(HttpMethod.Delete.ToString()) => CreateResponse(statusCode ?? StatusCodes.Status200OK,
                    "Resource deleted successfully.", data),
            _ => CreateResponse(statusCode ?? StatusCodes.Status200OK, "Request handled successfully.", data)
        };
    }

    private ApiResponseDto<object> CreateResponse(int statusCode, string message, object? data = null)
    {
        return new ApiResponseDto<object>(statusCode, message, data);
    }


    private ApiResponseDto<object> HandleGetResponse(object? data)
    {
        // Si `data` es nulo (por ejemplo, un recurso no encontrado)
        if (data == null) return CreateResponse(StatusCodes.Status400BadRequest, "Resource not found.");

        return data switch
        {
            ResponsePagedDto<object> pagedResult => CreateResponse(StatusCodes.Status200OK,
                "Paged data retrieved successfully.", pagedResult),
            IEnumerable<object> enumerable => CreateResponse(StatusCodes.Status200OK,
                "Enumerable retrieved successfully.", new ResponseDto<IEnumerable<object>>(enumerable)),
            ResponseComboBoxItemDto comboBox => CreateResponse(StatusCodes.Status200OK,
                "ComboBox retrieved successfully.", comboBox),
            _ => CreateResponse(StatusCodes.Status200OK, "Resource retrieved successfully.",
                new ResponseDto<object>(data))
        };
    }
}