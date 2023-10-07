using RabbitMQ.Client;

namespace Watermark.Services
{
    public class RabbitMQClientService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        public static string ExchangeName = "ImageDirectExchange";
        public static string RoutingWatermak = "watermark-route-image";
        public static string QueueName = "queue-watermark-image";

        private readonly ILogger<RabbitMQClientService> _logger;

    }
}
