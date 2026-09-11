using MicroservicioClientes.Domain.Enums;
using MicroservicioClientes.Domain.Primitives;
using MicroservicioClientes.Domain.ValueObjects;

namespace MicroservicioClientes.Domain.Clientes;

/// <summary>
/// Representa la información común de una persona dentro del dominio.
/// </summary>
public abstract class Persona : AggregateRoot
{
    /// <summary>
    /// Obtiene el identificador de la persona.
    /// </summary>
    public ClienteId Id { get; protected set; }

    /// <summary>
    /// Obtiene el nombre de la persona.
    /// </summary>
    public string Nombre { get; protected set; }

    /// <summary>
    /// Obtiene el género de la persona.
    /// </summary>
    public Genero Genero { get; protected set; }

    /// <summary>
    /// Obtiene la edad de la persona.
    /// </summary>
    public Edad Edad { get; protected set; }

    /// <summary>
    /// Obtiene la identificación de la persona.
    /// </summary>
    public Identificacion Identificacion { get; protected set; }

    /// <summary>
    /// Obtiene la dirección de la persona.
    /// </summary>
    public string Direccion { get; protected set; }

    /// <summary>
    /// Obtiene el número de teléfono de la persona.
    /// </summary>
    public Telefono Telefono { get; protected set; }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="Persona"/>.
    /// </summary>
    /// <param name="nombre">Nombre de la persona.</param>
    /// <param name="genero">Género de la persona.</param>
    /// <param name="edad">Edad de la persona.</param>
    /// <param name="identificacion">Identificación de la persona.</param>
    /// <param name="direccion">Dirección de la persona.</param>
    /// <param name="telefono">Número de teléfono de la persona.</param>
    protected Persona(
        string nombre,
        Genero genero,
        Edad edad,
        Identificacion identificacion,
        string direccion,
        Telefono telefono)
    {
        Nombre = nombre;
        Genero = genero;
        Edad = edad;
        Identificacion = identificacion;
        Direccion = direccion;
        Telefono = telefono;
    }

    protected Persona()
    {
    }
}