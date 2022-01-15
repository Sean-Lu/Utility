using System;

namespace Sean.Utility.Impls.Queue
{
    public class MessageReceivedEventArgs<T> : EventArgs
    {
        public T Data { get; }

        public MessageReceivedEventArgs(T data)
        {
            Data = data;
        }
    }
}