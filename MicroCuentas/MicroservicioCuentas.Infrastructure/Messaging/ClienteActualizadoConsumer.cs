using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MicroservicioCuentas.Application.Events;
using MicroservicioCuentas.Domain.ClientesReferencia;
using MicroservicioCuentas.Domain.Primitives;

namespace MicroservicioCuentas.Infrastructure.Messaging;

/// <summary>
/// Consume los eventos de actualización de clientes publicados mediante RabbitMQ
/// y mantiene sincronizada la referencia local del cliente.
/// </summary>
public class ClienteActualizadoConsumer : BackgroundService
{
    private readonly RabbitMQOptions _options;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ClienteActualizadoConsumer> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ClienteActualizadoConsumer"/>.
    /// </summary>
    /// <param name="options">Configuración de RabbitMQ.</param>
    /// <param name="scopeFactory">Fábrica para crear scopes de servicios.</param>
    /// <param name="logger">Componente utilizado para registrar eventos y errores.</param>
    public ClienteActualizadoConsumer(
        IOptions<RabbitMQOptions> options,
        IServiceScopeFactory scopeFactory,
        ILogger<ClienteActualizadoConsumer> logger)
    {
        _options = options.Value;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    /// <summary>
    /// Inicia el consumo de mensajes mientras la aplicación se encuentre activa.
    /// </summary>
    /// <param name="stoppingToken">Token utilizado para detener el consumidor.</param>
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consumer de clientes iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConsumeMessagesAsync(stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error en el consumer de clientes.");

                await Task.Delay(
                    TimeSpan.FromSeconds(5),
                    stoppingToken);
            }
        }

        _logger.LogInformation(
            "Consumer de clientes detenido.");
    }

    /// <summary>
    /// Configura la conexión con RabbitMQ y procesa los mensajes recibidos.
    /// </summary>
    /// <param name="stoppingToken">Token utilizado para cancelar el consumo.</param>
    private async Task ConsumeMessagesAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Conectando a RabbitMQ. Host={Host}, Port={Port}, Exchange={Exchange}, Queue={Queue}, RoutingKey={RoutingKey}.",
            _options.Host,
            _options.Port,
            _options.Exchange,
            _options.Queue,
            _options.RoutingKey);

        ConnectionFactory factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password
        };

        _logger.LogInformation(
            "Intentando conectar a RabbitMQ.");

        await using IConnection connection =
            await factory.CreateConnectionAsync(
                stoppingToken);

        _logger.LogInformation(
            "Conexión con RabbitMQ creada.");

        await using IChannel channel =
            await connection.CreateChannelAsync(
                cancellationToken: stoppingToken);

        _logger.LogInformation(
            "Channel de RabbitMQ creado.");

        await channel.ExchangeDeclareAsync(
            exchange: _options.Exchange,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: stoppingToken);

        _logger.LogInformation(
            "Exchange declarado.");

        await channel.QueueDeclareAsync(
            queue: _options.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken);

        _logger.LogInformation(
            "Queue declarada.");

        await channel.QueueBindAsync(
            queue: _options.Queue,
            exchange: _options.Exchange,
            routingKey: _options.RoutingKey,
            cancellationToken: stoppingToken);

        _logger.LogInformation(
            "Queue vinculada al exchange.");

        AsyncEventingBasicConsumer consumer =
            new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            _logger.LogInformation(
                "Mensaje recibido. DeliveryTag={DeliveryTag}.",
                args.DeliveryTag);

            try
            {
                string json =
                    Encoding.UTF8.GetString(
                        args.Body.ToArray());

                _logger.LogDebug(
                    "JSON recibido: {Json}.",
                    json);

                ClienteActualizadoEvent? evento =
                    JsonSerializer.Deserialize<ClienteActualizadoEvent>(
                        json);

                if (evento is null)
                {
                    _logger.LogError(
                        "El evento recibido no pudo ser deserializado.");

                    await channel.BasicNackAsync(
                        args.DeliveryTag,
                        multiple: false,
                        requeue: false,
                        cancellationToken: stoppingToken);

                    return;
                }

                _logger.LogInformation(
                    "Evento procesado. ClienteId={ClienteId}, Nombre={Nombre}, Estado={Estado}.",
                    evento.ClienteId,
                    evento.Nombre,
                    evento.Estado);

                using IServiceScope scope =
                    _scopeFactory.CreateScope();

                IClienteReferenciaRepository repository =
                    scope.ServiceProvider
                        .GetRequiredService<IClienteReferenciaRepository>();

                IUnitOfWork unitOfWork =
                    scope.ServiceProvider
                        .GetRequiredService<IUnitOfWork>();

                ClienteReferencia? clienteReferencia =
                    await repository.GetByIdAsync(
                        evento.ClienteId,
                        stoppingToken);

                if (clienteReferencia is null)
                {
                    _logger.LogInformation(
                        "ClienteReferencia no encontrada. Creando referencia para ClienteId={ClienteId}.",
                        evento.ClienteId);

                    clienteReferencia =
                        ClienteReferencia.Create(
                            evento.ClienteId,
                            evento.Estado,
                            evento.Nombre);

                    repository.Add(clienteReferencia);
                }
                else
                {
                    _logger.LogInformation(
                        "ClienteReferencia encontrada. Actualizando estado para ClienteId={ClienteId}.",
                        evento.ClienteId);

                    clienteReferencia.ActualizarEstado(
                        evento.Estado);
                }

                await unitOfWork.SaveChangesAsync(
                    stoppingToken);

                _logger.LogInformation(
                    "ClienteReferencia guardada correctamente. ClienteId={ClienteId}.",
                    evento.ClienteId);

                await channel.BasicAckAsync(
                    args.DeliveryTag,
                    multiple: false,
                    cancellationToken: stoppingToken);

                _logger.LogInformation(
                    "ACK enviado. DeliveryTag={DeliveryTag}.",
                    args.DeliveryTag);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error procesando el mensaje. DeliveryTag={DeliveryTag}.",
                    args.DeliveryTag);

                try
                {
                    await channel.BasicNackAsync(
                        args.DeliveryTag,
                        multiple: false,
                        requeue: true,
                        cancellationToken: stoppingToken);
                }
                catch (Exception nackException)
                {
                    _logger.LogError(
                        nackException,
                        "Error enviando NACK. DeliveryTag={DeliveryTag}.",
                        args.DeliveryTag);
                }
            }
        };

        await channel.BasicConsumeAsync(
            queue: _options.Queue,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation(
            "Consumer registrado. Esperando mensajes.");

        await Task.Delay(
            Timeout.Infinite,
            stoppingToken);
    }
}