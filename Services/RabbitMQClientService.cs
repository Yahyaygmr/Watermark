using RabbitMQ.Client;

namespace Watermark.Services
{
    public class RabbitMQClientService: IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        public static string ExchangeName = "ImageDirectExchange";
        public static string RoutingWatermak = "watermark-route-image";
        public static string QueueName = "queue-watermark-image";

        private readonly ILogger<RabbitMQClientService> _logger;

        public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger, IModel channel)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _channel = channel;
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();

            if (_channel is { IsOpen: true })
            {
                return _channel;
            }
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, type: "direct", true, false);
            _channel.QueueDeclare(QueueName, true, false, false, null);

            _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingWatermak);
            _logger.LogInformation("RabbitMQ ile Bağlantı Kuruldu...");

            return _channel;

        }

        public void Dispose()
        {
            _channel?.Close();
            _channel.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("RabbitMQ Bağlantısı Kapandı...");

        }
    }
}
