using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Sean.Utility.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Task"/>
    /// </summary>
    public static class TaskExtensions
    {
#if !NET40
        #region 让 Task 支持带超时的异步等待
        /// <summary>
        /// 让 Task 支持带超时的异步等待
        /// </summary>
        /// <param name="task">Task对象</param>
        /// <param name="timeout">超时时间设置</param>
        /// <param name="callback">回调方法（true：任务超时，false：任务没有超时）</param>
        /// <param name="throwExceptionWhenTimeout">如果任务超时，是否抛出异常：TimeoutException</param>
        /// <returns></returns>
        public static async Task WaitAsync(this Task task, TimeSpan timeout, Action<bool> callback, bool throwExceptionWhenTimeout = false)
        {
            using (var cts = new CancellationTokenSource())
            {
                var delayTask = Task.Delay(timeout, cts.Token);
                if (await Task.WhenAny(task, delayTask) == task)
                {
                    cts.Cancel();
                    callback?.Invoke(false);
                    return;
                }
            }

            DoTimeout(callback, throwExceptionWhenTimeout);
        }

        /// <summary>
        /// 让 Task 支持带超时的异步等待
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task">Task对象</param>
        /// <param name="timeout">超时时间设置</param>
        /// <param name="callback">回调方法（true：任务超时，false：任务没有超时）</param>
        /// <param name="throwExceptionWhenTimeout">如果任务超时，是否抛出异常：TimeoutException</param>
        /// <returns></returns>
        public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, TimeSpan timeout, Action<bool> callback, bool throwExceptionWhenTimeout = false)
        {
            using (var cts = new CancellationTokenSource())
            {
                var delayTask = Task.Delay(timeout, cts.Token);
                if (await Task.WhenAny(task, delayTask) == task)
                {
                    cts.Cancel();
                    callback?.Invoke(false);
                    return await task;
                }
            }

            DoTimeout(callback, throwExceptionWhenTimeout);

            //return (TResult)(object)"The task has timed out.";
            return default;
        }

        /// <summary>
        /// 让 Task 支持带超时的异步等待
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task">Task对象</param>
        /// <param name="timeoutMilliseconds">超时时间设置</param>
        /// <param name="callback">回调方法（true：任务超时，false：任务没有超时）</param>
        /// <param name="throwExceptionWhenTimeout">如果任务超时，是否抛出异常：TimeoutException</param>
        /// <returns></returns>
        public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, int timeoutMilliseconds, Action<bool> callback, bool throwExceptionWhenTimeout = false)
        {
            return await WaitAsync(task, TimeSpan.FromMilliseconds(timeoutMilliseconds), callback, throwExceptionWhenTimeout);
        }

        /// <summary>
        /// 任务超时的时候执行
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="throwExceptionWhenTimeout"></param>
        private static void DoTimeout(Action<bool> callback, bool throwExceptionWhenTimeout)
        {
            // 待优化：这里任务已超时，没有实现强制退出任务的功能。
            // 实际上任务还会在后台继续异步执行，直到任务结束。

            callback?.Invoke(true);
            if (throwExceptionWhenTimeout)
            {
                throw new TimeoutException("The operation has timed out.");
            }
        }
        #endregion

        #region 线程死锁
        /// <summary>
        /// 避免线程死锁：async\await + Task.Wait或Task.Result
        /// </summary>
        /// <param name="task"></param>
        public static ConfiguredTaskAwaitable PreventThreadDeadlock(this Task task)
        {
            return task.ConfigureAwait(false);
        }
        /// <summary>
        /// 避免线程死锁：async\await + Task.Wait或Task.Result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static ConfiguredTaskAwaitable<T> PreventThreadDeadlock<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false);
        }
        #endregion
#endif

        #region Task异常捕获
        /// <summary>
        /// Task异常捕获
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="logMsg"></param>
        /// <param name="handleError"></param>
        /// <returns></returns>
        public static Task<T> TaskEx<T>(this Task<T> task, string logMsg = null, Action<string, Exception> handleError = null)
        {
            return task.ContinueWith(c =>
            {
                if (!c.IsFaulted)
                    return c.Result;

                c.Exception?.InnerExceptions?.ToList().ForEach(ex =>
                {
                    handleError?.Invoke(logMsg ?? string.Empty, ex);
                });
                return default;
            });
        }

        /// <summary>
        /// Task异常捕获
        /// </summary>
        /// <param name="task"></param>
        /// <param name="logMsg"></param>
        /// <param name="handleError"></param>
        /// <returns></returns>
        public static Task TaskEx(this Task task, string logMsg = null, Action<string, Exception> handleError = null)
        {
            return task.ContinueWith(c =>
            {
                if (!c.IsFaulted)
                    return;

                c.Exception?.InnerExceptions?.ToList().ForEach(ex =>
                {
                    handleError?.Invoke(logMsg ?? string.Empty, ex);
                });
            });
        }

        /// <summary>
        /// Task异常捕获
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="logMsg"></param>
        /// <param name="handleError"></param>
        /// <returns></returns>
        public static T TaskResultEx<T>(this Task<T> task, string logMsg = null, Action<string, Exception> handleError = null)
        {
            return task.TaskEx(logMsg, handleError).Result;
        }
        #endregion
    }
}
