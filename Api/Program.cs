using Api.Extensions;
using Application;
using Application.Filters;
using Persistence;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceInfraestructure(builder.Configuration);
builder.Services.AddApplicationShared(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseFilter>();
});

builder.Services.AddApiVersioningExtension();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseErrorHandlingMiddleware();
app.MapControllers();

app.Run();