using Rmq.WorkerLib;
using System;
using System.Threading;

namespace Rmq.Worker1
{
    class Program
    {
        const string WorkerName = "WORKER1";
        const string HostName = "localhost";
        const string QueueName = "task_queue1";

        static void Main(string[] args)
        {
            Console.WriteLine(WorkerName);

            var work = new Work(HostName, QueueName);
            work.Execute(DoAction);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        static TaskResult DoAction(string message)
        {
            var args = new MessageArgs(message);

            var result = args.A + args.B;

            Thread.Sleep(result % 2 == 0 ? 1000 : 5000);

            var printedResult = WorkerHelpers.PrintResult(args.TaskId, string.Format("{0}+{1}={2}", args.A, args.B, result));

            return new TaskResult()
            {
                TaskId = args.TaskId,
                SenderRoute = args.SenderRoute + ".w1",
                Result = printedResult,
            };
        }
    }
}
