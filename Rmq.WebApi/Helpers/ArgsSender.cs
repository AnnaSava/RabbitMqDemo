using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Rmq.TaskerLib;
using System;

namespace Rmq.WebApi.Helpers
{
    public class ArgsSender
    {
        private readonly string _hostname;
        private IConnection _connection;

        public ArgsSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _hostname = rabbitMqOptions.Value.Hostname;

            CreateConnection();
        }

        public void Send(ArgsTask args)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: args.Queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    channel.BasicPublish(exchange: "", routingKey: args.Queue, basicProperties: null, body: args.Body);
                }
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    ClientProvidedName = ClientProvidedNames.ArgsComponentName,
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}
