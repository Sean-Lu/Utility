using System;
using System.Threading.Tasks;

namespace Sean.Utility.Common
{
    /// <summary>
    /// 委托相关
    /// </summary>
    public class DelegateHelper
    {
        /// <summary>
        /// 执行指定任务（重试机制）
        /// </summary>
        /// <param name="func">如果返回false（表示执行失败），根据参数<paramref name="retryCnt"/>尝试重试</param>
        /// <param name="retryCnt">重试次数，0表示不重试</param>
        /// <returns></returns>
        public static bool RetryFunc(Func<bool> func, int retryCnt = 0)
        {
            if (func == null)
            {
                return false;
            }

            for (var i = 0; i < Math.Max(1, retryCnt + 1); i++)
            {
                if (func())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 执行指定任务（分批多次重复执行，常用于数据量非常大的情况）
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="BatchFunc"/> method.
        /// <code>
        /// var execCnt = 0;
        /// DelegateHelper.BatchFunc(() =>
        /// {
        ///     Console.WriteLine(DateTime.Now.ToLongDateTimeString());
        ///     Thread.Sleep(1000);
        ///     if (++execCnt >= 3)
        ///     {
        ///         return true;
        ///     }
        ///     return false;
        /// }, null);
        /// </code>
        /// </example>
        /// <param name="func">如果返回true，则表示执行结束（退出循环）</param>
        /// <param name="maxExecCount">最大执行次数，null表示不限制（慎用，防止出现死循环）</param>
        public static void BatchFunc(Func<bool> func, int? maxExecCount = 100)
        {
            if (func == null)
            {
                return;
            }

            var execCnt = 0;
            do
            {
                execCnt++;
                if (func())
                {
                    //执行结束
                    break;
                }
            } while (!maxExecCount.HasValue || execCnt < maxExecCount.Value);
        }

#if !NET40
        /// <summary>
        /// 执行指定任务（重试机制）
        /// </summary>
        /// <param name="func">如果返回false（表示执行失败），根据参数<paramref name="retryCnt"/>尝试重试</param>
        /// <param name="retryCnt">重试次数，0表示不重试</param>
        /// <returns></returns>
        public static async Task<bool> RetryFuncAsync(Func<Task<bool>> func, int retryCnt = 0)
        {
            if (func == null)
            {
                return false;
            }

            for (var i = 0; i < Math.Max(1, retryCnt + 1); i++)
            {
                if (await func())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 执行指定任务（分批多次重复执行，常用于数据量非常大的情况）
        /// </summary>
        /// <param name="func">如果返回true，则表示执行结束（退出循环）</param>
        /// <param name="maxExecCount">最大执行次数，null表示不限制（慎用，防止出现死循环）</param>
        public static async Task BatchFuncAsync(Func<Task<bool>> func, int? maxExecCount = 100)
        {
            if (func == null)
            {
                return;
            }

            var execCnt = 0;
            do
            {
                execCnt++;
                if (await func())
                {
                    //执行结束
                    break;
                }
            } while (!maxExecCount.HasValue || execCnt < maxExecCount.Value);
        }
#endif
    }
}
