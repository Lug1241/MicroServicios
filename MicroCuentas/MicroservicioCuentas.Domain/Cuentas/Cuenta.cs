using MicroservicioCuentas.Domain.Enums;
using MicroservicioCuentas.Domain.Primitives;
using MicroservicioCuentas.Domain.ValueObjets;

namespace MicroservicioCuentas.Domain.Cuentas;

/// <summary>
/// Representa una cuenta bancaria dentro del dominio.
/// </summary>
public class Cuenta : AggregateRoot
{
    /// <summary>
    /// Obtiene el identificador de la cuenta.
    /// </summary>
    public CuentaId Id { get; private set; }

    /// <summary>
    /// Obtiene el número de cuenta.
    /// </summary>
    public int NumeroCuenta { get; private set; }

    /// <summary>
    /// Obtiene el tipo de cuenta.
    /// </summary>
    public TipoCuenta TipoCuenta { get; private set; }

    /// <summary>
    /// Obtiene el saldo inicial de la cuenta.
    /// </summary>
    public SaldoInicial SaldoInicial { get; private set; }

    /// <summary>
    /// Obtiene el estado actual de la cuenta.
    /// </summary>
    public bool Estado { get; private set; }

    /// <summary>
    /// Obtiene el identificador del cliente propietario de la cuenta.
    /// </summary>
    public int ClienteId { get; private set; }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="Cuenta"/>.
    /// </summary>
    /// <param name="numeroCuenta">Número de la cuenta.</param>
    /// <param name="tipoCuenta">Tipo de cuenta.</param>
    /// <param name="saldoInicial">Saldo inicial de la cuenta.</param>
    /// <param name="estado">Estado inicial de la cuenta.</param>
    /// <param name="clienteId">Identificador del cliente propietario.</param>
    public Cuenta(
        int numeroCuenta,
        TipoCuenta tipoCuenta,
        SaldoInicial saldoInicial,
        bool estado,
        int clienteId)
    {
        NumeroCuenta = numeroCuenta;
        TipoCuenta = tipoCuenta;
        SaldoInicial = saldoInicial;
        Estado = estado;
        ClienteId = clienteId;
    }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="Cuenta"/>.
    /// </summary>
    public Cuenta()
    {
    }

    /// <summary>
    /// Crea una nueva cuenta.
    /// </summary>
    /// <param name="numeroCuenta">Número de la cuenta.</param>
    /// <param name="tipoCuenta">Tipo de cuenta.</param>
    /// <param name="saldoInicial">Saldo inicial de la cuenta.</param>
    /// <param name="estado">Estado inicial de la cuenta.</param>
    /// <param name="clienteId">Identificador del cliente propietario.</param>
    /// <returns>Una nueva instancia de <see cref="Cuenta"/>.</returns>
    public static Cuenta Create(
        int numeroCuenta,
        TipoCuenta tipoCuenta,
        SaldoInicial saldoInicial,
        bool estado,
        int clienteId)
    {
        Cuenta cuenta = new Cuenta(
            numeroCuenta,
            tipoCuenta,
            saldoInicial,
            estado,
            clienteId);

        return cuenta;
    }

    /// <summary>
    /// Activa la cuenta.
    /// </summary>
    public void Activar() =>
        Estado = true;

    /// <summary>
    /// Inactiva la cuenta.
    /// </summary>
    public void Inactivar() =>
        Estado = false;
}