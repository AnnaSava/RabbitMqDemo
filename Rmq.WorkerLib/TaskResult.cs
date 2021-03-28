namespace Rmq.WorkerLib
{
    public class TaskResult
    {
        public int TaskId { get; set; }

        public string SenderRoute { get; set; }

        public string Result { get; set; }
    }
}
