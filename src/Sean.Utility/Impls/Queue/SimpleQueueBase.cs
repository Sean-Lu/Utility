using System;

namespace Sean.Utility.Impls.Queue
{
    public abstract class SimpleQueueBase
    {
#if NETSTANDARD
        internal static IServiceProvider ServiceProvider { get; set; }
#endif
    }
}
