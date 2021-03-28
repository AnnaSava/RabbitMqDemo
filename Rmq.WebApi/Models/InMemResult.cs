namespace Rmq.WebApi.Models
{
    public class InMemResult
    {
        public string RoutingKey { get; }

        public string Message { get; }

        public InMemResult(string routingKey, string message)
        {
            RoutingKey = routingKey;
            Message = message;
        }
    }
}
