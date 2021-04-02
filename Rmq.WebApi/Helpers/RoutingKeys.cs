namespace Rmq.WebApi.Helpers
{
    public class RoutingKeys
    {
        public const string Random = "webapi.random";
        public const string All = "webapi.all";

        public static string FormatKey(string action, string worker)
        {
            return string.Format("webapi.{0}.w{1}", action, worker);
        }
    }
}
