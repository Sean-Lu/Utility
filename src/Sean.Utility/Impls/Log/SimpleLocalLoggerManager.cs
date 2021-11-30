using System.Diagnostics;

namespace Sean.Utility.Impls.Log
{
    public class SimpleLocalLoggerManager
    {
        public static SimpleLocalLogger GetCurrentClassLogger()
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(1);
            var methodBase = stackFrame.GetMethod();
            return new SimpleLocalLogger(methodBase.ReflectedType);
        }
    }
}
