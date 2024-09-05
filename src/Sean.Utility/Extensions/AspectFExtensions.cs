using System;
using System.Diagnostics;
using Sean.Utility.AOP;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;

namespace Sean.Utility.Extensions;

public static class AspectFExtensions
{
    [DebuggerStepThrough]
    public static AspectF Delay(this AspectF aspect, int milliseconds)
    {
        return aspect.Combine((work) =>
        {
            if (milliseconds > 0)
            {
                System.Threading.Thread.Sleep(milliseconds);
            }
            work();
        });
    }

    [DebuggerStepThrough]
    public static AspectF MustNotNull(this AspectF aspect, params object[] args)
    {
        return aspect.Combine((work) =>
        {
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == null)
                    {
                        throw new ArgumentNullException(nameof(args), $"Parameter at index {i} is null.");
                    }
                }
            }

            work();
        });
    }

    [DebuggerStepThrough]
    public static AspectF Until(this AspectF aspect, Func<bool> test, int? testDelayMilliseconds = null)
    {
        return aspect.Combine((work) =>
        {
            while (!test())
            {
                if (testDelayMilliseconds.HasValue && testDelayMilliseconds.Value > 0)
                {
                    System.Threading.Thread.Sleep(testDelayMilliseconds.Value);
                }
            }

            work();
        });
    }

    [DebuggerStepThrough]
    public static AspectF While(this AspectF aspect, Func<bool> test, int? testDelayMilliseconds = null)
    {
        return aspect.Combine((work) =>
        {
            while (test())
            {
                work();

                if (testDelayMilliseconds.HasValue && testDelayMilliseconds.Value > 0)
                {
                    System.Threading.Thread.Sleep(testDelayMilliseconds.Value);
                }
            }
        });
    }

    [DebuggerStepThrough]
    public static AspectF WhenTrue(this AspectF aspect, params Func<bool>[] conditions)
    {
        return aspect.Combine((work) =>
        {
            foreach (Func<bool> condition in conditions)
                if (!condition())
                    return;

            work();
        });
    }

    [DebuggerStepThrough]
    public static AspectF WhenAnyTrue(this AspectF aspect, params Func<bool>[] conditions)
    {
        return aspect.Combine((work) =>
        {
            foreach (Func<bool> condition in conditions)
            {
                if (condition())
                {
                    work();
                    return;
                }
            }
        });
    }

    [DebuggerStepThrough]
    public static AspectF WhenFalse(this AspectF aspect, params Func<bool>[] conditions)
    {
        return aspect.Combine((work) =>
        {
            foreach (Func<bool> condition in conditions)
                if (condition())
                    return;

            work();
        });
    }

    [DebuggerStepThrough]
    public static AspectF WhenAnyFalse(this AspectF aspect, params Func<bool>[] conditions)
    {
        return aspect.Combine((work) =>
        {
            foreach (Func<bool> condition in conditions)
            {
                if (!condition())
                {
                    work();
                    return;
                }
            }
        });
    }

    [DebuggerStepThrough]
    public static AspectF RunAsync(this AspectF aspect, Action completeCallback)
    {
        return aspect.Combine((work) => work.BeginInvoke(asyncResult =>
        {
            work.EndInvoke(asyncResult);
            completeCallback();
        }, null));
    }

    [DebuggerStepThrough]
    public static AspectF RunAsync(this AspectF aspect)
    {
        return aspect.Combine((work) => work.BeginInvoke(asyncResult =>
        {
            work.EndInvoke(asyncResult);
        }, null));
    }

    #region Retry
    [DebuggerStepThrough]
    public static AspectF RetryWhenError(this AspectF aspect, int retryCount, Action<int, Exception> errorHandler, int? retryDuration = null)
    {
        var totalRetryCount = retryCount;
        return aspect.Combine((work) =>
        {
            do
            {
                try
                {
                    work();
                    return;
                }
                catch (Exception ex)
                {
                    errorHandler?.Invoke(totalRetryCount - retryCount, ex);
                    if (retryDuration.HasValue && retryDuration.Value > 0)
                    {
                        System.Threading.Thread.Sleep(retryDuration.Value);
                    }
                }
            } while (retryCount-- > 0);
        });
    }
    #endregion

    #region Logger
    [DebuggerStepThrough]
    public static AspectF Log(this AspectF aspect, Action<string> before, Action<string> after, string beforeMessage, string afterMessage)
    {
        return aspect.Combine((work) =>
        {
            if (!string.IsNullOrEmpty(beforeMessage))
            {
                before?.Invoke(beforeMessage);
            }

            work();

            if (!string.IsNullOrEmpty(afterMessage))
            {
                after?.Invoke(afterMessage);
            }
        });
    }
    [DebuggerStepThrough]
    public static AspectF LogBegin(this AspectF aspect, Action<string> action, string message)
    {
        return aspect.Log(action, null, message, null);
    }
    [DebuggerStepThrough]
    public static AspectF LogEnd(this AspectF aspect, Action<string> action, string message)
    {
        return aspect.Log(null, action, null, message);
    }

    [DebuggerStepThrough]
    public static AspectF Log(this AspectF aspect, ILogger logger, LogLevel logLevel, string beforeMessage, string afterMessage)
    {
        return aspect.Log(msg => Log(logger, logLevel, msg), msg => Log(logger, logLevel, msg), beforeMessage, afterMessage);
    }
    [DebuggerStepThrough]
    public static AspectF LogBegin(this AspectF aspect, ILogger logger, LogLevel logLevel, string message)
    {
        return aspect.Log(logger, logLevel, message, null);
    }
    [DebuggerStepThrough]
    public static AspectF LogEnd(this AspectF aspect, ILogger logger, LogLevel logLevel, string message)
    {
        return aspect.Log(logger, logLevel, null, message);
    }

    [DebuggerStepThrough]
    public static AspectF LogDebug(this AspectF aspect, ILogger logger, string beforeMessage, string afterMessage)
    {
        return aspect.Log(logger, LogLevel.Debug, beforeMessage, afterMessage);
    }
    [DebuggerStepThrough]
    public static AspectF LogInfo(this AspectF aspect, ILogger logger, string beforeMessage, string afterMessage)
    {
        return aspect.Log(logger, LogLevel.Info, beforeMessage, afterMessage);
    }

    [DebuggerStepThrough]
    public static AspectF LogError(this AspectF aspect, ILogger logger, string errMsg = null, bool throwException = true)
    {
        return aspect.Combine((work) =>
        {
            try
            {
                work();
            }
            catch (Exception ex)
            {
                logger?.LogError(errMsg ?? ex.Message, ex);
                if (throwException)
                {
                    throw;
                }
            }
        });
    }
    [DebuggerStepThrough]
    public static AspectF IgnoreError(this AspectF aspect)
    {
        return aspect.Combine((work) =>
        {
            try
            {
                work();
            }
            catch (Exception)
            {
                // ignored
            }
        });
    }
    #endregion

    private static void Log(ILogger logger, LogLevel logLevel, string message)
    {
        switch (logLevel)
        {
            case LogLevel.Debug:
                logger?.LogDebug(message);
                break;
            case LogLevel.Info:
                logger?.LogInfo(message);
                break;
            case LogLevel.Warn:
                logger?.LogWarn(message);
                break;
            case LogLevel.Error:
                logger?.LogError(message);
                break;
            case LogLevel.Fatal:
                logger?.LogFatal(message);
                break;
            default:
                throw new NotImplementedException();
        }
    }
}