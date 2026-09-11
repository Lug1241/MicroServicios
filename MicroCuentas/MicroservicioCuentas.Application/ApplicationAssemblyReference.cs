using System.Reflection;

namespace MicroservicioCuentas.Application;

/// <summary>
/// Proporciona una referencia al assembly de la capa de aplicación.
/// </summary>
public class ApplicationAssemblyReference
{
    /// <summary>
    /// Obtiene el assembly que contiene esta clase.
    /// </summary>
    internal static readonly Assembly Assembly =
        typeof(ApplicationAssemblyReference).Assembly;
}