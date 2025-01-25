using Application.Middlewares;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Extensions;

public static class AppExtensions
{
    public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseMiddleware<TimezoneMiddleware>();
    }
}