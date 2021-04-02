namespace Rmq.WebApi
{
    public class RabbitMqConfiguration
    {
        public string Hostname { get; set; }

        public string LoggerExchangeName { get; set; }

        public string ResultExchangeName { get; set; }

        public string ListenActions { get; set; }

        public string ListenWorkers { get; set; }
    }
}
