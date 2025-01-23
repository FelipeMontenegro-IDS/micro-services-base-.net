using Api.Extensions;
using Application;
using Application.Filters;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Persistence;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddApplicationShared(builder.Configuration);
builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddControllers(options => { options.Filters.Add<ApiResponseFilter>(); });

// builder.Services.AddHealthChecks()
//     .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty,
//         name: "sqlserver",
//         failureStatus: HealthStatus.Degraded)
//     .AddCheck("api_health_check", () => HealthCheckResult.Healthy("API is running"));

// builder.Services.AddHealthChecksUI(options =>
// {
//     options.SetEvaluationTimeInSeconds(10); // Tiempo entre evaluaciones
//     options.MaximumHistoryEntriesPerEndpoint(50); // Histórico
//     options.AddHealthCheckEndpoint("API Health Check", "/healthz");
// }).AddInMemoryStorage();

builder.Services.AddApiVersioningExtension();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Servir archivos estáticos antes de mapear la UI de HealthChecks
app.UseStaticFiles();

// Middleware para redireccionar HTTP a HTTPS
app.UseHttpsRedirection();

// Manejo de autorización y errores personalizados
app.UseAuthorization();
app.UseErrorHandlingMiddleware();

// Endpoint para HealthChecks (API principal)
// app.MapHealthChecks("/healthz", new HealthCheckOptions
// {
//     Predicate = _ => true,
//     AllowCachingResponses = false,
//     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
//     ResultStatusCodes =
//     {
//         [HealthStatus.Healthy] = StatusCodes.Status200OK,
//         [HealthStatus.Degraded] = StatusCodes.Status200OK,
//         [HealthStatus.Unhealthy] = StatusCodes.Status500InternalServerError
//     }
// });

// Endpoint para HealthChecks UI
// app.MapHealthChecksUI(options =>
// {
//     options.ApiPath = "/health-ui-api"; // Ruta para la API interna del UI
//     options.UIPath = "/dashboard";      // Ruta para el Dashboard
// });

// Endpoint para controladores
app.MapControllers();

// Logging (opcional)
builder.Logging.AddConsole();
builder.Logging.AddDebug();

app.Run();
