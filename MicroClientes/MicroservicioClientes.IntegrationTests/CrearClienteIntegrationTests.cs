using System.Net;
using System.Net.Http.Json;
using Microsoft.Data.SqlClient;

namespace MicroservicioClientes.IntegrationTests;

/// <summary>
/// Prueba la creación de un cliente y la propagación de su información
/// hacia el microservicio de cuentas mediante RabbitMQ.
/// </summary>
public class CrearClienteIntegrationTests
{
    private const string ApiUrl = "http://localhost:5154";

    private const string ClientesConnectionString =
        "Server=localhost,1434;" +
        "Database=ClientesDB;" +
        "User Id=sa;" +
        "Password=Base123!;" +
        "TrustServerCertificate=True;";

    private const string CuentasConnectionString =
        "Server=localhost,1434;" +
        "Database=CuentasDB;" +
        "User Id=sa;" +
        "Password=Base123!;" +
        "TrustServerCertificate=True;";

    /// <summary>
    /// Verifica que la creación de un cliente persista la información en ClientesDB
    /// y que el evento publicado mediante RabbitMQ cree su referencia en CuentasDB.
    /// </summary>
    [Fact]
    public async Task CrearCliente_DebeCrearClienteYPublicarEvento()
    {
        using HttpClient client = new HttpClient
        {
            BaseAddress = new Uri(ApiUrl)
        };

        ClienteRequest request = new ClienteRequest(
            "Cliente Test Integracion",
            1,
            25,
            "0602771297",
            "Direccion de prueba",
            "0999999999",
            "Test123");

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/Clientes",
            request);

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        ClienteResponse? cliente = await response.Content
            .ReadFromJsonAsync<ClienteResponse>();

        Assert.NotNull(cliente);

        Assert.True(cliente.Id > 0);

        Assert.Equal(
            "Cliente Test Integracion",
            cliente.Nombre);

        Assert.Equal(
            "0602771297",
            cliente.Identificacion);

        Assert.True(cliente.Estado);

        await VerificarClienteEnBaseDatos(
            cliente.Id,
            request.Nombre,
            request.Identificacion);

        await Task.Delay(5000);

        await VerificarClienteReferenciaEnBaseDatos(
            cliente.Id,
            request.Nombre);
    }

    /// <summary>
    /// Verifica que el cliente haya sido persistido correctamente en ClientesDB.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="nombre">Nombre esperado del cliente.</param>
    /// <param name="identificacion">Identificación esperada del cliente.</param>
    private static async Task VerificarClienteEnBaseDatos(
        int clienteId,
        string nombre,
        string identificacion)
    {
        await using SqlConnection connection =
            new SqlConnection(ClientesConnectionString);

        await connection.OpenAsync();

        const string sql = """
            SELECT
                p.Id,
                p.Nombre,
                p.Identificacion,
                c.Estado
            FROM Persona p
            INNER JOIN Cliente c
                ON c.Id = p.Id
            WHERE p.Id = @ClienteId
            """;

        await using SqlCommand command =
            new SqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "@ClienteId",
            clienteId);

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        Assert.True(
            await reader.ReadAsync(),
            "El cliente no fue encontrado en ClientesDB.");

        Assert.Equal(
            clienteId,
            reader.GetInt32(
                reader.GetOrdinal("Id")));

        Assert.Equal(
            nombre,
            reader.GetString(
                reader.GetOrdinal("Nombre")));

        Assert.Equal(
            identificacion,
            reader.GetString(
                reader.GetOrdinal("Identificacion")));

        Assert.True(
            reader.GetBoolean(
                reader.GetOrdinal("Estado")));
    }

    /// <summary>
    /// Verifica que la referencia del cliente haya sido creada en CuentasDB
    /// después del procesamiento del evento de RabbitMQ.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="nombre">Nombre esperado del cliente.</param>
    private static async Task VerificarClienteReferenciaEnBaseDatos(
        int clienteId,
        string nombre)
    {
        await using SqlConnection connection =
            new SqlConnection(CuentasConnectionString);

        await connection.OpenAsync();

        const string sql = """
            SELECT
                ClienteId,
                Nombre,
                Estado
            FROM ClienteReferencia
            WHERE ClienteId = @ClienteId
            """;

        await using SqlCommand command =
            new SqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "@ClienteId",
            clienteId);

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        Assert.True(
            await reader.ReadAsync(),
            "El ClienteReferencia no fue creado en CuentasDB.");

        Assert.Equal(
            clienteId,
            reader.GetInt32(
                reader.GetOrdinal("ClienteId")));

        Assert.Equal(
            nombre,
            reader.GetString(
                reader.GetOrdinal("Nombre")));

        Assert.True(
            reader.GetBoolean(
                reader.GetOrdinal("Estado")));
    }

    /// <summary>
    /// Representa la información enviada al endpoint de creación de clientes.
    /// </summary>
    private sealed record ClienteRequest(
        string Nombre,
        int Genero,
        int Edad,
        string Identificacion,
        string Direccion,
        string Telefono,
        string Contraseña);

    /// <summary>
    /// Representa la respuesta devuelta por el endpoint de creación de clientes.
    /// </summary>
    private sealed record ClienteResponse(
        int Id,
        string Nombre,
        int Genero,
        int Edad,
        string Identificacion,
        string Direccion,
        string Telefono,
        bool Estado);
}