using System.Collections.Concurrent;
using System.Threading;

namespace Sean.Utility.Contracts
{
    public interface ISimpleLocalQueue<T>
    {
        ConcurrentQueue<T> Queue { get; }
        AutoResetEvent WaitHandler { get; }
    }
}