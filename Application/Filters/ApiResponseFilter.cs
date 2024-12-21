using Application.Wrappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
            var httpMethod = context.HttpContext.Request.Method; // Tipo de método HTTP (GET, POST, PUT, DELETE)
            var data = objectResult.Value; 
            ApiResponse<object> apiResponse;
            
            switch (httpMethod)
            {
                case "GET":
                    apiResponse = HandleGetResponse(data);
                    break;

                case "POST":
                    apiResponse = new ApiResponse<object>(
                        objectResult.StatusCode ?? 201,
                        "Resource created successfully.",
                        data
                    );
                    break;

                case "PUT":
                    apiResponse = new ApiResponse<object>(
                        objectResult.StatusCode ?? 200,
                        "Resource updated successfully.",
                        data
                    );
                    break;

                case "DELETE":
                    apiResponse = new ApiResponse<object>(
                        objectResult.StatusCode ?? 200,
                        "Resource deleted successfully."
                    );
                    break;

                default:
                    apiResponse = new ApiResponse<object>(
                        objectResult.StatusCode ?? 200,
                        "Request handled successfully.",
                        data
                    );
                    break;
            }

            // Reemplaza el resultado con la estructura ApiResponse
            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = objectResult.StatusCode
            };

        }
    }
    
    private ApiResponse<object> HandleGetResponse(object data)
    {
        // Si `data` es nulo (por ejemplo, un recurso no encontrado)
        if (data == null)
        {
            return new ApiResponse<object>(
                404,
                "Resource not found.",
                null
            );
        }

        // Detectar si es "paginación", "arreglo de datos", o un objeto único
        if (data is ResponsePagedResult<Object> pagedResult) // Manejo de paginación
        {
            return new ApiResponse<object>(
                200,
                "Paged data retrieved successfully.",
                pagedResult
            );
        }
        else if (data is IEnumerable<object> list) // Manejo de listas
        {
            return new ApiResponse<object>(
                200,
                "List retrieved successfully.",
                list
            );
        }
        else // Manejo de objetos únicos
        {
            return new ApiResponse<object>(
                200,
                "Resource retrieved successfully.",
                data
            );
        }
    }
}