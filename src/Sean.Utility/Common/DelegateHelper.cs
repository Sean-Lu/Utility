using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sean.Utility
{
    public class DelegateHelper
    {
        /// <summary>
        /// 执行任务（重试机制）
        /// </summary>
        /// <param name="func">返回true表示执行成功，返回false表示执行失败。</param>
        /// <param name="retryCount">重试次数</param>
        /// <returns></returns>
        public static bool Retry(Func<bool> func, int retryCount)
        {
            if (func == null)
            {
                return false;
            }

            do
            {
                if (func())
                {
                    return true;
                }
            } while (retryCount-- > 0);

            return false;
        }

        /// <summary>
        /// 重复执行任务
        /// </summary>
        /// <param name="func">如果返回true，则表示执行结束</param>
        /// <param name="maxRepeatCount">最大执行次数</param>
        public static void Repeat(Func<bool> func, int maxRepeatCount)
        {
            if (func == null)
            {
                return;
            }

            do
            {
                if (func())
                {
                    break;
                }
            } while (--maxRepeatCount > 0);
        }

#if !NET40
        /// <summary>
        /// 异步执行任务（重试机制）
        /// </summary>
        /// <param name="func">返回true表示执行成功，返回false表示执行失败。</param>
        /// <param name="retryCount">重试次数</param>
        /// <returns></returns>
        public static async Task<bool> RetryAsync(Func<Task<bool>> func, int retryCount)
        {
            if (func == null)
            {
                return false;
            }

            do
            {
                if (await func())
                {
                    return true;
                }
            } while (retryCount-- > 0);

            return false;
        }

        /// <summary>
        /// 异步重复执行任务
        /// </summary>
        /// <param name="func">如果返回true，则表示执行结束</param>
        /// <param name="maxRepeatCount">最大执行次数</param>
        public static async Task RepeatAsync(Func<Task<bool>> func, int maxRepeatCount)
        {
            if (func == null)
            {
                return;
            }

            do
            {
                if (await func())
                {
                    break;
                }
            } while (--maxRepeatCount > 0);
        }
#endif
    }
}
