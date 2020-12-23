using System.Threading;

namespace Sean.Utility.Threading
{
    /// <summary>
    /// Thread
    /// </summary>
    public class ThreadHelper
    {
        /// <summary>
        /// Get current thread
        /// </summary>
        /// <returns></returns>
        public static Thread GetCurrentThread()
        {
            return Thread.CurrentThread;
        }
        /// <summary>
        /// Get the id of current thread
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentThreadId()
        {
            return GetCurrentThread().ManagedThreadId;
        }
    }
}
