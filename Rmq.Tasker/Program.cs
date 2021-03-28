using RabbitMQ.Client;
using Rmq.TaskerLib;
using System;
using System.Linq;
using System.Text;

namespace Rmq.Tasker
{
    class Program
    {
        const string HostName = "localhost";
        const string LoggerExchangeName = "logger";
        const string RoutingKey = "tasker.random";

        static void Main(string[] args)
        {
            Console.WriteLine("TASKER");
            Console.WriteLine("Press [enter] to create and send a task.");

            var factory = new ConnectionFactory() { HostName = HostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    using (var logChannel = connection.CreateModel())
                    {
                        while (Console.ReadKey().Key == ConsoleKey.Enter)
                        {
                            var newTask = CreateTask();

                            channel.QueueDeclare(queue: newTask.Queue,
                                         durable: true, // When RabbitMQ quits or crashes it will forget the queues and messages unless you tell it not to. 
                                                        //Two things are required to make sure that messages aren't lost: we need to mark both the queue and messages as durable.
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                            channel.BasicPublish(exchange: "",
                                                 routingKey: newTask.Queue,
                                                 basicProperties: null,
                                                 body: newTask.Body);

                            logChannel.ExchangeDeclare(exchange: LoggerExchangeName, type: ExchangeType.Fanout);

                            logChannel.BasicPublish(exchange: LoggerExchangeName,
                                                 routingKey: "",
                                                 basicProperties: null,
                                                 body: Encoding.UTF8.GetBytes(string.Format("[{0}] Task {1} started.", DateTime.Now, newTask.TaskId)));

                            PrintInfo(newTask);
                        }
                    }
                }
            }

            Console.WriteLine(" Press any key to exit.");
            Console.ReadLine();

        }

        static ArgsTask CreateTask()
        {
            return TaskerHelper.GenerateRandom(RoutingKey).First();
        }

        static void PrintInfo(ArgsTask task)
        {
            Console.WriteLine(" [{0}] Task {1} Sent to {2} {3}", DateTime.Now, task.TaskId, task.Queue, task.Message);
        }
    }
}
