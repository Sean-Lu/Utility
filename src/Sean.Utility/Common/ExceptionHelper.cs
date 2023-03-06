using System;
using System.Threading;
using System.Threading.Tasks;
using Sean.Utility.Contracts;

namespace Sean.Utility
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class ExceptionHelper
    {
        /// <summary>
        /// 全局异常捕获：
        /// <para>AppDomain.CurrentDomain.UnhandledException</para>
        /// <para>TaskScheduler.UnobservedTaskException</para>
        /// </summary>
        /// <param name="logger"></param>
        public static void CatchGlobalUnhandledException(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            CatchGlobalUnhandledException((ex, msg) =>
            {
                logger.LogError(msg, ex);
            });
        }
        /// <summary>
        /// 全局异常捕获：
        /// <para>AppDomain.CurrentDomain.UnhandledException</para>
        /// <para>TaskScheduler.UnobservedTaskException</para>
        /// </summary>
        /// <param name="logger"></param>
        public static void CatchGlobalUnhandledException(ILoggerAsync logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            CatchGlobalUnhandledException((ex, msg) =>
            {
                logger.LogErrorAsync(msg, ex);
            });
        }
        /// <summary>
        /// 全局异常捕获：
        /// <para>AppDomain.CurrentDomain.UnhandledException</para>
        /// <para>TaskScheduler.UnobservedTaskException</para>
        /// </summary>
        /// <param name="handleError"></param>
        public static void CatchGlobalUnhandledException(Action<Exception, string> handleError)
        {
            if (handleError == null)
            {
                return;
            }

            #region UI线程异常
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //Application.ThreadException += (sender, e) =>
            //{
            //    handleError(e?.Exception, "Application.ThreadException");
            //};
            #endregion

            AppDomain.CurrentDomain.UnhandledException += (obj, e) =>
            {
                if (e?.ExceptionObject is Exception ex)
                {
                    handleError(ex, "AppDomain.CurrentDomain.UnhandledException");
                }
            };

            TaskScheduler.UnobservedTaskException += (obj, e) =>
            {
                e?.SetObserved();
                e?.Exception?.Flatten().Handle(ex =>
                {
                    handleError(ex, "TaskScheduler.UnobservedTaskException");
                    return true;
                });
            };
        }

        /// <summary>
        /// try{...} catch{...} final{...}
        /// </summary>
        /// <param name="tryToDo">在try里面执行的委托</param>
        /// <param name="handleError">处理异常的委托，如果委托返回true则会Rethrow exception</param>
        /// <param name="finalToDo">最后都会执行的委托（无论是否有异常）</param>
        public static void TryCatchFinal(Action tryToDo, Func<Exception, bool> handleError = null, Action finalToDo = null)
        {
            try
            {
                tryToDo?.Invoke();
            }
            catch (Exception ex)
            {
                if (handleError != null && handleError(ex))
                {
                    throw;
                }
            }
            finally
            {
                finalToDo?.Invoke();
            }
        }
        /// <summary>
        /// try{...} catch{...} final{...}
        /// </summary>
        /// <param name="tryToDo">在try里面执行的委托</param>
        /// <param name="handleError">处理异常的委托</param>
        /// <param name="finalToDo">最后都会执行的委托（无论是否有异常）</param>
        /// <param name="rethrowException">如果有异常，是否抛出异常</param>
        public static T TryCatchFinal<T>(Func<T> tryToDo, Func<Exception, T> handleError = null, Action finalToDo = null, bool rethrowException = false)
        {
            try
            {
                return tryToDo != null ? tryToDo() : default;
            }
            catch (Exception ex)
            {
                if (rethrowException)
                {
                    throw;
                }
                else
                {
                    return handleError != null ? handleError(ex) : default;
                }
            }
            finally
            {
                finalToDo?.Invoke();
            }
        }

#if !NET40
        /// <summary>
        /// try{...} catch{...} final{...}
        /// </summary>
        /// <param name="tryToDo">在try里面执行的委托</param>
        /// <param name="handleError">处理异常的委托，如果委托返回true则会Rethrow exception</param>
        /// <param name="finalToDo">最后都会执行的委托（无论是否有异常）</param>
        public static async Task TryCatchFinalAsync(Func<Task> tryToDo, Func<Exception, bool> handleError = null, Action finalToDo = null)
        {
            try
            {
                if (tryToDo != null)
                {
                    await tryToDo();
                }
            }
            catch (Exception ex)
            {
                if (handleError != null && handleError(ex))
                {
                    throw;
                }
            }
            finally
            {
                finalToDo?.Invoke();
            }
        }
        /// <summary>
        /// try{...} catch{...} final{...}
        /// </summary>
        /// <param name="tryToDo">在try里面执行的委托</param>
        /// <param name="handleError">处理异常的委托</param>
        /// <param name="finalToDo">最后都会执行的委托（无论是否有异常）</param>
        /// <param name="rethrowException">如果有异常，是否抛出异常</param>
        public static async Task<T> TryCatchFinalAsync<T>(Func<Task<T>> tryToDo, Func<Exception, Task<T>> handleError = null, Action finalToDo = null, bool rethrowException = false)
        {
            try
            {
                if (tryToDo != null)
                {
                    return await tryToDo();
                }

                return default;
            }
            catch (Exception ex)
            {
                if (rethrowException)
                {
                    throw;
                }
                else
                {
                    if (handleError != null)
                    {
                        return await handleError(ex);
                    }

                    return default;
                }
            }
            finally
            {
                finalToDo?.Invoke();
            }
        }
#endif
    }
}
