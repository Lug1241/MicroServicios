using System.Net;
using System.Text.Json;
using FluentValidation;
using MicroservicioCuentas.Domain;

namespace MicroservicioCuentas.Api.Middlewares;

/// <summary>
/// Middleware encargado de capturar las excepciones de la aplicación
/// y transformarlas en respuestas HTTP con un formato consistente.
/// </summary>
public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de
    /// <see cref="GlobalExceptionHandlingMiddleware"/>.
    /// </summary>
    /// <param name="logger">
    /// Logger utilizado para registrar las excepciones.
    /// </param>
    public GlobalExceptionHandlingMiddleware(
        ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Procesa una solicitud HTTP y captura las excepciones producidas
    /// durante su ejecución.
    /// </summary>
    /// <param name="context">Contexto de la solicitud HTTP.</param>
    /// <param name="next">Siguiente componente del pipeline HTTP.</param>
    /// <returns>Una tarea que representa la ejecución del middleware.</returns>
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
    /// Construye y escribe la respuesta HTTP correspondiente a una excepción.
    /// </summary>
    /// <param name="context">Contexto de la solicitud HTTP.</param>
    /// <param name="statusCode">Código de estado HTTP de la respuesta.</param>
    /// <param name="title">Título de la respuesta.</param>
    /// <param name="detail">Detalle del error.</param>
    /// <param name="errors">Lista opcional de errores específicos.</param>
    /// <returns>Una tarea que representa la escritura de la respuesta.</returns>
    private static async Task WriteResponse(
        HttpContext context,
        HttpStatusCode statusCode,
        string title,
        string detail,
        IEnumerable<string>? errors = null)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        object problem = new
        {
            Status = (int)statusCode,
            Type = title,
            Title = title,
            Detail = detail,
            Errors = errors
        };

        string json = JsonSerializer.Serialize(problem);

        await context.Response.WriteAsync(json);
    }
}