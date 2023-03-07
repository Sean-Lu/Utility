using System;

namespace Sean.Utility.Impls.Queue
{
    public abstract class SimpleQueueBase
    {
#if NETSTANDARD || NET5_0_OR_GREATER
        internal static IServiceProvider ServiceProvider { get; set; }
#endif
    }
}
