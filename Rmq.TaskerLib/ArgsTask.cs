using System.Text;

namespace Rmq.TaskerLib
{
    public class ArgsTask
    {
        public int TaskId { get; }

        public string Queue { get; }

        public string Message { get; }

        public byte[] Body { get; }

        public ArgsTask(int taskId, string queue, string message)
        {
            TaskId = taskId;
            Queue = queue;
            Body = Encoding.UTF8.GetBytes(message);
            Message = message;
        }
    }
}
