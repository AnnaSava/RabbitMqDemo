using System;
using System.Text;

namespace Rmq.WorkerLib
{
    public static class WorkerHelpers
    {
        public static string PrintResult(int taskId, string formattedResult)
        {
            var result = string.Format(" [{0}] Task {1} {2}", DateTime.Now, taskId, formattedResult);
            Console.WriteLine(result);
            return result;
        }

        public static void PrintError(Exception e)
        {
            Console.WriteLine("[{0}] Error: {1}", DateTime.Now, e.Message);
        }

        public static byte[] GetFinishedLogBody(int taskId)
        {
            return Encoding.UTF8.GetBytes(string.Format("[{0}] Task {1} finished.", DateTime.Now, taskId));
        }

        public static byte[] GetResultBody(string result)
        {
            return Encoding.UTF8.GetBytes(result);
        }
    }
}
