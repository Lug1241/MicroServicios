using MicroservicioCuentas.Api;
using MicroservicioCuentas.Api.Extensions;
using MicroservicioCuentas.Api.Middlewares;
using MicroservicioCuentas.Application;
using MicroservicioCuentas.Infrastructure;

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

app.UseHttpsRedirection();


app.MapControllers();

app.ApplyMigrations();

app.Run();