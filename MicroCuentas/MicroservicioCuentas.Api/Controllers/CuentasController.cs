using MediatR;
using MicroservicioCuentas.Application.Cuentas;
using MicroservicioCuentas.Application.Cuentas.Create;
using MicroservicioCuentas.Application.Cuentas.GetById;
using MicroservicioCuentas.Application.Cuentas.Update;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicioCuentas.Api.Controllers;

/// <summary>
/// Expone los endpoints HTTP relacionados con las cuentas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CuentasController : ControllerBase
{
    private readonly ISender _sender;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="CuentasController"/>.
    /// </summary>
    /// <param name="sender">
    /// Componente utilizado para enviar solicitudes a la capa de aplicación.
    /// </param>
    public CuentasController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Crea una nueva cuenta.
    /// </summary>
    /// <param name="command">
    /// Datos necesarios para crear la cuenta.
    /// </param>
    /// <param name="cancellationToken">
    /// Token para cancelar la operación.
    /// </param>
    /// <returns>
    /// Información de la cuenta creada.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCuentaCommand command,
        CancellationToken cancellationToken)
    {
        CuentaResponse response =
            await _sender.Send(
                command,
                cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Obtiene una cuenta mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador de la cuenta.</param>
    /// <param name="cancellationToken">
    /// Token para cancelar la operación.
    /// </param>
    /// <returns>
    /// Información de la cuenta consultada.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        GetCuentaByIdQuery query =
            new GetCuentaByIdQuery(id);

        CuentaResponse response =
            await _sender.Send(
                query,
                cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Actualiza el estado de una cuenta.
    /// </summary>
    /// <param name="id">Identificador de la cuenta.</param>
    /// <param name="command">
    /// Datos utilizados para actualizar el estado de la cuenta.
    /// </param>
    /// <param name="cancellationToken">
    /// Token para cancelar la operación.
    /// </param>
    /// <returns>
    /// Una respuesta sin contenido cuando la actualización se realiza correctamente.
    /// </returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCuentaCommand command,
        CancellationToken cancellationToken)
    {
        command = command with { Id = id };

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }
}