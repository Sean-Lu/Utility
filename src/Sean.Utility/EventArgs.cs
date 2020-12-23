using System;

namespace Sean.Utility
{
    public class EventArgs<T> : EventArgs
    {
        public T Data;

        public EventArgs(T data)
        {
            Data = data;
        }
    }
}
