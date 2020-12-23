using System;
using System.Timers;

namespace Sean.Utility.Timers
{
    /// <summary>
    /// <see cref="Timer"/> 定时器
    /// </summary>
    public class TimerExt : TimerBase
    {
        /// <summary>
        /// 定时执行的委托
        /// </summary>
        public Action Execute { get; set; }

        public TimerExt(double interval, bool autoReset = true, bool disallowConcurrentExecution = true) : base(interval, autoReset, disallowConcurrentExecution)
        {

        }

        protected override void Elapsed()
        {
            Execute?.Invoke();
        }
    }
}
