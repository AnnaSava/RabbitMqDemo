using System;
using System.Collections.Generic;

namespace Rmq.TaskerLib
{
    public static class TaskerHelper
    {
        const string QueueNameFormat = "task_queue{0}";
        const string MessageFormat = "{0},{1},a:{2},b:{3}";

        public static int TaskId { get; private set; }

        public static List<ArgsTask> GenerateRandom(string senderRoute, int argsCount = 1, int minVal = 1, int maxVal = 100)
        {
            var rand = new Random();

            var tasks = new List<ArgsTask>();

            for (int i = 0; i < argsCount; i++)
            {
                var queue = string.Format(QueueNameFormat, rand.Next(1, 4));
                int taskId = TaskId++;
                var message = string.Format(MessageFormat, senderRoute, taskId, rand.Next(minVal, maxVal + 1), rand.Next(minVal, maxVal + 1));

                tasks.Add(new ArgsTask(taskId, queue, message));
            }

            return tasks;
        }

        public static List<ArgsTask> GenerateFromStrings(string senderRoute, IEnumerable<string> strings)
        {
            var tasks = new List<ArgsTask>();

            foreach (var s in strings)
            {
                var args = s.Split(",");
                if (args.Length != 2) continue;

                for (int i = 1; i <= 3; i++)
                {
                    var queue = string.Format(QueueNameFormat, i);
                    int taskId = TaskId++;
                    var message = string.Format(MessageFormat, senderRoute, taskId, args[0], args[1]);

                    tasks.Add(new ArgsTask(taskId, queue, message));
                }
            }

            return tasks;
        }
    }
}
