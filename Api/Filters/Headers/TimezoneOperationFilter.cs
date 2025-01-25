using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Filters.Headers;

public class TimezoneOperationFilter : IOperationFilter
{
    
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
        {
            operation.Parameters = new List<OpenApiParameter>();
        }
        
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "timezone",
            In = ParameterLocation.Header,
            Required = true,
            Description = "Timezone for request (e.g. America/New_York)",
            Schema = new OpenApiSchema
            {
                Type = "string"
            }
        });
    }
}