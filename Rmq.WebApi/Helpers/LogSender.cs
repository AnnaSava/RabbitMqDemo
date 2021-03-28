using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Rmq.TaskerLib;
using System;
using System.Text;

namespace Rmq.WebApi.Helpers
{
    public class LogSender
    {
        private readonly string _hostname;
        private readonly string _queuename;
        private IConnection _connection;

        public LogSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
            _queuename = rabbitMqOptions.Value.LoggerExchangeName;

            CreateConnection();
        }

        public void Send(ArgsTask args)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: _queuename, type: ExchangeType.Fanout);

                    channel.BasicPublish(exchange: _queuename,
                                         routingKey: "",
                                         basicProperties: null,
                                         body: Encoding.UTF8.GetBytes(string.Format("[{0}] Task {1} started.", DateTime.Now, args.TaskId)));
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
                    ClientProvidedName = ClientProvidedNames.LogComponentName,
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
