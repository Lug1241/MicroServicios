using MicroservicioClientes.Domain.Enums;
using MicroservicioClientes.Domain.ValueObjects;

namespace MicroservicioClientes.Domain.Clientes;

/// <summary>
/// Representa un cliente dentro del dominio.
/// </summary>
public sealed class Cliente : Persona
{
    /// <summary>
    /// Obtiene la contraseña del cliente.
    /// </summary>
    public string Contraseña { get; private set; }

    /// <summary>
    /// Obtiene el estado actual del cliente.
    /// </summary>
    public bool Estado { get; private set; }

    private Cliente(
        string nombre,
        Genero genero,
        Edad edad,
        Identificacion identificacion,
        string direccion,
        Telefono telefono,
        string contraseña,
        bool estado)
        : base(
            nombre,
            genero,
            edad,
            identificacion,
            direccion,
            telefono)
    {
        Contraseña = contraseña;
        Estado = estado;
    }

    private Cliente()
    {
    }

    /// <summary>
    /// Crea un nuevo cliente activo.
    /// </summary>
    /// <param name="nombre">Nombre del cliente.</param>
    /// <param name="genero">Género del cliente.</param>
    /// <param name="edad">Edad del cliente.</param>
    /// <param name="identificacion">Identificación del cliente.</param>
    /// <param name="direccion">Dirección del cliente.</param>
    /// <param name="telefono">Número de teléfono del cliente.</param>
    /// <param name="contraseña">Contraseña del cliente.</param>
    /// <returns>Una nueva instancia de <see cref="Cliente"/>.</returns>
    public static Cliente Create(
        string nombre,
        Genero genero,
        Edad edad,
        Identificacion identificacion,
        string direccion,
        Telefono telefono,
        string contraseña)
    {
        Cliente cliente = new Cliente(
            nombre,
            genero,
            edad,
            identificacion,
            direccion,
            telefono,
            contraseña,
            true);

        return cliente;
    }

    /// <summary>
    /// Actualiza la información del cliente.
    /// </summary>
    /// <param name="nombre">Nuevo nombre del cliente.</param>
    /// <param name="genero">Nuevo género del cliente.</param>
    /// <param name="edad">Nueva edad del cliente.</param>
    /// <param name="identificacion">Nueva identificación del cliente.</param>
    /// <param name="direccion">Nueva dirección del cliente.</param>
    /// <param name="telefono">Nuevo número de teléfono del cliente.</param>
    /// <param name="contraseña">Nueva contraseña del cliente.</param>
    public void Update(
        string nombre,
        Genero genero,
        Edad edad,
        Identificacion identificacion,
        string direccion,
        Telefono telefono,
        string contraseña)
    {
        Nombre = nombre;
        Genero = genero;
        Edad = edad;
        Identificacion = identificacion;
        Telefono = telefono;
        Direccion = direccion;
        Contraseña = contraseña;
    }

    /// <summary>
    /// Activa el cliente.
    /// </summary>
    public void Activar() => Estado = true;

    /// <summary>
    /// Inactiva el cliente.
    /// </summary>
    public void Inactivar() => Estado = false;
}