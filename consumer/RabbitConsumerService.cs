using System.Net;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SignalRTelemetry.Controllers;

namespace SignalRTelemetry.Rabbit;

public class RabbitConsumerService : BackgroundService
{

    private IServiceProvider _sp;
    private ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;

    private const string TELEMETRY_CONTROLLER_URI="https://localhost:7249/Telemetry";

    public RabbitConsumerService(IServiceProvider sp)
    {
        _sp = sp;
        // Configure RabbitMQ Client
        _factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "telemetry",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);

            // Allow untrusted certificates
            // DON'T USE IN PRODUCTION!!!!!!!!!!!!!
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };

            var client = new HttpClient(handler);

            int parseResult;

            if (int.TryParse(message, out parseResult))
            {
                var data = new TelemetryData();

                data.Decibels = int.Parse(message);
                if (data.Decibels<0 || data.Decibels>150)  {
                    Console.WriteLine($"Decibels data {data.Decibels} is not valid.");
                }

                var result = client.PostAsJsonAsync<TelemetryData>(TELEMETRY_CONTROLLER_URI, data).Result;                
            }
            else  {
                Console.WriteLine($"Decibels data {message} is not valid.");
            }
        };
        _channel.BasicConsume(queue: "telemetry",
                             autoAck: true,
                             consumer: consumer);

        return Task.CompletedTask;
    }
}