using Rmq.WebApi.Models;
using System.Collections.Generic;

namespace Rmq.WebApi
{
    public static class ImMemResultsStorage
    {
        public static List<InMemResult> Results { get; set; } = new List<InMemResult>();
    }
}
