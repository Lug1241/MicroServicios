using Microsoft.EntityFrameworkCore;
using MicroservicioCuentas.Domain.ClientesReferencia;

namespace MicroservicioCuentas.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementa las operaciones de persistencia relacionadas con las referencias de clientes.
/// </summary>
public class ClienteReferenciaRepository : IClienteReferenciaRepository
{
    private readonly CuentasDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ClienteReferenciaRepository"/>.
    /// </summary>
    /// <param name="context">Contexto de persistencia de cuentas.</param>
    public ClienteReferenciaRepository(CuentasDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene una referencia de cliente mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>
    /// La referencia encontrada o <see langword="null"/> si no existe.
    /// </returns>
    public async Task<ClienteReferencia?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.ClientesReferencia
            .FirstOrDefaultAsync(
                cliente => cliente.ClienteId == id,
                cancellationToken);
    }

    /// <summary>
    /// Agrega una nueva referencia de cliente al contexto de persistencia.
    /// </summary>
    /// <param name="cliente">Referencia del cliente que se desea agregar.</param>
    public void Add(ClienteReferencia cliente)
    {
        _context.ClientesReferencia.Add(cliente);
    }
}