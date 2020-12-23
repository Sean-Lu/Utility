using System;
using System.Threading.Tasks;

namespace Sean.Utility.Threading
{
    /// <summary>
    /// Task
    /// </summary>
    public class TaskHelper
    {
#if !NET40
        /// <summary>
        /// 延时执行指定任务
        /// </summary>
        /// <param name="millisecondsDelay">延时时间（单位：毫秒）</param>
        /// <param name="action">待执行的任务</param>
        /// <returns></returns>
        public static async Task Delay(int millisecondsDelay, Action action)
        {
            if (action == null || millisecondsDelay < 0)
            {
                return;
            }

            await Task.Delay(millisecondsDelay);// 需要Framework 4.5+
            action();
        }
#endif
    }
}
