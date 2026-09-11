using System.Reflection;

namespace MicroservicioClientes.Api;

/// <summary>
/// Proporciona una referencia al assembly de la capa de presentación.
/// </summary>
public class PresentationAssemblyReference
{
    /// <summary>
    /// Obtiene el assembly que contiene esta clase.
    /// </summary>
    internal static readonly Assembly Assembly =
        typeof(PresentationAssemblyReference).Assembly;
}