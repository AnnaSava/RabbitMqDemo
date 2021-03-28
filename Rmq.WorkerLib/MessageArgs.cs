using System;

namespace Rmq.WorkerLib
{
    public class MessageArgs
    {
        public string SenderRoute { get; set; }

        public int TaskId { get; }

        public int A { get; }

        public int B { get; }

        public MessageArgs(string message)
        {
            var parameters = message.Split(",");
            if (parameters.Length != 4) throw new Exception("Incorrect args!");

            int a = 0, b = 0;

            SenderRoute = parameters[0];

            TaskId = int.Parse(parameters[1]);

            for (int i = 2; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var keyvals = parameter.Split(":");
                if (keyvals.Length != 2) throw new Exception("Incorrect vals!");
                FillValue(keyvals, nameof(a), ref a);
                FillValue(keyvals, nameof(b), ref b);
            }

            A = a;
            B = b;
        }

        void FillValue(string[] keyvals, string keyName, ref int variable)
        {
            if (keyvals[0] == keyName) variable = int.Parse(keyvals[1]);
        }
    }
}
