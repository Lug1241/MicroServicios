using MicroservicioClientes.Domain.Clientes;
using MicroservicioClientes.Domain.Enums;
using MicroservicioClientes.Domain.ValueObjects;

namespace MicroservicioClientes.Tests;

/// <summary>
/// Contiene pruebas unitarias para el agregado <see cref="Cliente"/>.
/// </summary>
public class ClienteTests
{
    /// <summary>
    /// Verifica que un cliente nuevo sea creado con estado activo.
    /// </summary>
    [Fact]
    public void Create_DebeCrearClienteActivo()
    {
        Edad edad = Edad.Create(32);
        Identificacion identificacion = Identificacion.Create("1726313255");
        Telefono telefono = Telefono.Create("0982547851");

        Cliente cliente = Cliente.Create(
            "Jose Lema",
            Genero.Masculino,
            edad,
            identificacion,
            "Otavalo sn y principal",
            telefono,
            "1234");

        Assert.True(cliente.Estado);
    }

    /// <summary>
    /// Verifica que un cliente activo pueda ser inactivado correctamente.
    /// </summary>
    [Fact]
    public void Inactivar_DebeDejarClienteInactivo()
    {
        Edad edad = Edad.Create(32);
        Identificacion identificacion = Identificacion.Create("1726313255");
        Telefono telefono = Telefono.Create("0982547851");

        Cliente cliente = Cliente.Create(
            "Jose Lema",
            Genero.Masculino,
            edad,
            identificacion,
            "Otavalo sn y principal",
            telefono,
            "1234");

        cliente.Inactivar();

        Assert.False(cliente.Estado);
    }
}