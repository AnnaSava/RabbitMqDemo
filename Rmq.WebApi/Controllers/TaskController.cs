using Microsoft.AspNetCore.Mvc;
using Rmq.TaskerLib;
using Rmq.WebApi.Helpers;
using Rmq.WebApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Rmq.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ArgsSender _argsSender;
        private readonly LogSender _logSender;

        public TaskController(ArgsSender argsSender, LogSender logSender)
        {
            _argsSender = argsSender;
            _logSender = logSender;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult RandomActions(RandomActionsQuery model)
        {
            var tasks = TaskerHelper.GenerateRandom(RoutingKeys.Random, model.Count, model.Min, model.Max);
            Send(tasks);
            return new JsonResult(new { message = "Tasks are sent" });
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AllActions(AllActionsQuery model)
        {
            var tasks = TaskerHelper.GenerateFromStrings(RoutingKeys.All, model.Args);
            Send(tasks);
            return new JsonResult(new { message = "Tasks are sent" });
        }

        [HttpGet]
        [Route("[action]")]

        public IActionResult GetResults(string actionType, string workerId)
        {
            var routingKey = RoutingKeys.FormatKey(actionType, workerId);
            var results = ImMemResultsStorage.Results.Where(m => m.RoutingKey == routingKey);
            return new JsonResult(results);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CleanResults()
        {
            ImMemResultsStorage.Results.Clear();
            return new JsonResult(new { message = "Results are cleaned"});
        }

        private void Send(IEnumerable<ArgsTask> tasks)
        {
            foreach (var task in tasks)
            {
                _argsSender.Send(task);
                _logSender.Send(task);
            }
        }
    }
}
