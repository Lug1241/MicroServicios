using FluentValidation;
using MicroservicioClientes.Domain;
using System.Net;
using System.Text.Json;

namespace MicroservicioClientes.Api.Middlewares;

/// <summary>
/// Middleware encargado de capturar y transformar las excepciones de la aplicación
/// en respuestas HTTP con un formato consistente.
/// </summary>
public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="GlobalExceptionHandlingMiddleware"/>.
    /// </summary>
    /// <param name="logger">
    /// Logger utilizado para registrar las excepciones capturadas.
    /// </param>
    public GlobalExceptionHandlingMiddleware(
        ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Ejecuta el middleware y gestiona las excepciones producidas durante
    /// el procesamiento de la solicitud.
    /// </summary>
    /// <param name="context">Contexto HTTP de la solicitud actual.</param>
    /// <param name="next">Siguiente middleware del pipeline.</param>
    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(
                ex,
                "Error de validación.");

            await WriteResponse(
                context,
                HttpStatusCode.BadRequest,
                "Validation Error",
                "La solicitud contiene datos inválidos.",
                ex.Errors.Select(error => error.ErrorMessage));
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(
                ex,
                "Error de dominio.");

            await WriteResponse(
                context,
                HttpStatusCode.BadRequest,
                "Domain Error",
                ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(
                ex,
                "Recurso no encontrado.");

            await WriteResponse(
                context,
                HttpStatusCode.NotFound,
                "Not Found",
                ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Ha ocurrido un error no controlado.");

            await WriteResponse(
                context,
                HttpStatusCode.InternalServerError,
                "Server Error",
                "Ha ocurrido un error interno en el servidor.");
        }
    }

    /// <summary>
    /// Escribe una respuesta HTTP con información sobre la excepción capturada.
    /// </summary>
    /// <param name="context">Contexto HTTP de la solicitud actual.</param>
    /// <param name="statusCode">Código de estado HTTP de la respuesta.</param>
    /// <param name="title">Título del error.</param>
    /// <param name="detail">Descripción del error.</param>
    /// <param name="errors">
    /// Lista opcional de errores específicos asociados a la respuesta.
    /// </param>
    private static async Task WriteResponse(
        HttpContext context,
        HttpStatusCode statusCode,
        string title,
        string detail,
        IEnumerable<string>? errors = null)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        string json = JsonSerializer.Serialize(
            new
            {
                Status = (int)statusCode,
                Type = title,
                Title = title,
                Detail = detail,
                Errors = errors
            });

        await context.Response.WriteAsync(json);
    }
}