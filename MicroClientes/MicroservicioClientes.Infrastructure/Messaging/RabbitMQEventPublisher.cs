using MicroservicioClientes.Application.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MicroservicioClientes.Infrastructure.Messaging;

/// <summary>
/// Publica eventos de la aplicación mediante RabbitMQ.
/// </summary>
public class RabbitMQEventPublisher : IEventPublisher
{
    private readonly RabbitMQOptions _options;

    private const int MaxRetries = 3;
    private const int RetryDelaySeconds = 2;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="RabbitMQEventPublisher"/>.
    /// </summary>
    /// <param name="options">Configuración de RabbitMQ.</param>
    public RabbitMQEventPublisher(
        IOptions<RabbitMQOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Publica un evento en RabbitMQ.
    /// </summary>
    /// <typeparam name="T">Tipo del evento.</typeparam>
    /// <param name="evento">Evento que se desea publicar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    public async Task PublishAsync<T>(
        T evento,
        CancellationToken cancellationToken = default)
    {
        Exception? lastException = null;

        for (int attempt = 1; attempt <= MaxRetries; attempt++)
        {
            try
            {
                await PublishInternalAsync(
                    evento,
                    cancellationToken);

                return;
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                lastException = ex;

                if (attempt == MaxRetries)
                {
                    break;
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(RetryDelaySeconds),
                    cancellationToken);
            }
        }

        throw new InvalidOperationException(
            "No fue posible publicar el evento en RabbitMQ después de varios intentos.",
            lastException);
    }

    private async Task PublishInternalAsync<T>(
        T evento,
        CancellationToken cancellationToken)
    {
        ConnectionFactory factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password
        };

        await using IConnection connection =
            await factory.CreateConnectionAsync(
                cancellationToken);

        await using IChannel channel =
            await connection.CreateChannelAsync(
                cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: _options.Exchange,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: _options.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: _options.Queue,
            exchange: _options.Exchange,
            routingKey: _options.RoutingKey,
            cancellationToken: cancellationToken);

        string json = JsonSerializer.Serialize(evento);

        byte[] body = Encoding.UTF8.GetBytes(json);

        BasicProperties properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json"
        };

        await channel.BasicPublishAsync(
            exchange: _options.Exchange,
            routingKey: _options.RoutingKey,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }
}