using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Rmq.WebApi.Models;
using System.Text;

namespace Rmq.WebApi.Helpers
{
    public class ResultListener
    {
        private readonly string resultExchangeName;

        ConnectionFactory factory { get; set; }
        IConnection connection { get; set; }
        IModel channel { get; set; }

        public ResultListener(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            factory = new ConnectionFactory() { HostName = rabbitMqOptions.Value.Hostname };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            resultExchangeName = rabbitMqOptions.Value.ResultExchangeName;
        }
        public void Register()
        {
            channel.ExchangeDeclare(exchange: resultExchangeName, type: ExchangeType.Topic);

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: resultExchangeName,
                              routingKey: RoutingKeys.Random);

            channel.QueueBind(queue: queueName,
                              exchange: resultExchangeName,
                              routingKey: RoutingKeys.All);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                ImMemResultsStorage.Results.Add(new InMemResult(ea.RoutingKey, message));
            };
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void Deregister()
        {
            connection.Close();
        }
    }
}
