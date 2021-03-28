using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Rmq.WorkerLib
{
    public class Work
    {
        private readonly string hostName;
        private readonly string queueName;
        const string LoggerExchangeName = "logger";
        const string ResultExchangeName = "result";

        public Work(string hostName, string queueName)
        {
            this.hostName = hostName;
            this.queueName = queueName;
        }

        public void Execute(Func<string, TaskResult> callbackAction)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    using (var logChannel = connection.CreateModel())
                    {
                        using (var resChannel = connection.CreateModel())
                        {
                            channel.QueueDeclare(queue: queueName,
                                                 durable: true,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);

                            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                            logChannel.ExchangeDeclare(exchange: LoggerExchangeName, type: ExchangeType.Fanout);

                            resChannel.ExchangeDeclare(exchange: ResultExchangeName, type: ExchangeType.Topic);

                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, ea) =>
                            {
                                var body = ea.Body.ToArray();
                                var message = Encoding.UTF8.GetString(body);

                                try
                                {
                                    var taskResult = callbackAction(message);
                                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                                    resChannel.BasicPublish(exchange: ResultExchangeName,
                                                         routingKey: taskResult.SenderRoute,
                                                         basicProperties: null,
                                                         body: WorkerHelpers.GetResultBody(taskResult.Result));

                                    logChannel.BasicPublish(exchange: LoggerExchangeName,
                                                         routingKey: taskResult.SenderRoute,
                                                         basicProperties: null,
                                                         body: WorkerHelpers.GetFinishedLogBody(taskResult.TaskId));
                                }
                                catch (Exception e)
                                {
                                    WorkerHelpers.PrintError(e);
                                }

                            };

                            channel.BasicConsume(queue: queueName,
                                                 autoAck: false,
                                                 consumer: consumer);

                            Console.WriteLine(" Press [enter] to exit.");
                            Console.ReadLine();
                        }
                    }
                }
            }
        }
    }
}
