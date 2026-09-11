using MicroservicioClientes.Api;
using MicroservicioClientes.Api.Extensions;
using MicroservicioClientes.Api.Middlewares;
using MicroservicioClientes.Application;
using MicroservicioClientes.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddPresentation();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.ApplyMigrations();

app.Run();