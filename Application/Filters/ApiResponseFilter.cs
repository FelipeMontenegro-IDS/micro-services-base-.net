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
            ApiResponseDto<object> apiResponse = CreateApiResponse(httpMethod,objectResult.StatusCode, data);
            
            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = objectResult.StatusCode
            };
        }
    }

    private ApiResponseDto<Object> CreateApiResponse(string httpMethod, int? statusCode, object data)
    {
        return httpMethod switch
        {
            var method when
                method == HttpMethod.Get.Method => HandleGetResponse(data),
            var method when
                method.Equals(HttpMethod.Post.ToString()) => CreateResponse(statusCode ?? 201, "Resource created successfully.", data),
            var method when
                method.Equals(HttpMethod.Put.ToString()) => CreateResponse(statusCode ?? 200, "Resource updated successfully.", data),
            var method when
                method.Equals(HttpMethod.Delete.ToString()) => CreateResponse(statusCode ?? 200, "Resource deleted successfully.", data),
            _ => CreateResponse(statusCode ?? 200, "Request handled successfully.", data)
        };
    }

    private ApiResponseDto<object> CreateResponse(int statusCode, string message, object? data = null)
    {
        return new ApiResponseDto<object>(statusCode, message, data);
    }


    private ApiResponseDto<object> HandleGetResponse(object? data)
    {
        // Si `data` es nulo (por ejemplo, un recurso no encontrado)
        if (data == null) return CreateResponse(400,  "Resource not found.");

        return data switch
        {
            ResponsePagedDto<object> pagedResult => CreateResponse(200, "Paged data retrieved successfully.", pagedResult),
            IEnumerable<object> enumerable => CreateResponse(200, "Enumerable retrieved successfully.", new ResponseDto< IEnumerable<object>>(enumerable)),
            ResponseComboBoxItemDto comboBox => CreateResponse(200, "ComboBox retrieved successfully.", comboBox),
            _ => CreateResponse(200, "Resource retrieved successfully.", new ResponseDto< object>(data))
        };
        
    }
}