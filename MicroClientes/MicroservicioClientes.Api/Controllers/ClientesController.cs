using MediatR;
using Microsoft.AspNetCore.Mvc;
using MicroservicioClientes.Application.Clientes.Create;
using MicroservicioClientes.Application.Clientes.Delete;
using MicroservicioClientes.Application.Clientes.GetById;
using MicroservicioClientes.Application.Clientes.Update;
using MicroservicioClientes.Application.Clientes;

namespace MicroservicioClientes.API.Controllers;

/// <summary>
/// Expone los endpoints HTTP relacionados con los clientes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ISender _mediator;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ClientesController"/>.
    /// </summary>
    /// <param name="mediator">
    /// Componente utilizado para enviar las solicitudes a la capa de aplicación.
    /// </param>
    public ClientesController(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene un cliente mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Los datos del cliente solicitado.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        ClienteResponse cliente = await _mediator.Send(
            new GetClienteByIdQuery(id),
            cancellationToken);

        return Ok(cliente);
    }

    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    /// <param name="command">Datos necesarios para crear el cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Los datos del cliente creado.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateClienteCommand command,
        CancellationToken cancellationToken)
    {
        ClienteResponse cliente = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(cliente);
    }

    /// <summary>
    /// Actualiza un cliente existente.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="command">Datos actualizados del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Una respuesta sin contenido cuando la actualización es exitosa.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateClienteCommand command,
        CancellationToken cancellationToken)
    {
        command = command with { Id = id };

        await _mediator.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Inactiva un cliente existente.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Una respuesta sin contenido cuando la operación es exitosa.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteClienteCommand(id),
            cancellationToken);

        return NoContent();
    }
}