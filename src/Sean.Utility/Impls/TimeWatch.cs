using System.Diagnostics;
using Sean.Utility.Contracts;

namespace Sean.Utility.Impls
{
    /// <summary>
    /// 计时器（<see cref="Stopwatch"/>）
    /// </summary>
    public class TimeWatch : ITimeWatch
    {
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// 计时器
        /// </summary>
        public TimeWatch()
        {
            _stopwatch = new Stopwatch();
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            _stopwatch.Start();
        }

        /// <summary>
        /// 重新开始计时
        /// </summary>
        public void Restart()
        {
            _stopwatch.Restart();
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop()
        {
            _stopwatch.Stop();
        }

        /// <summary>
        /// 返回耗时，单位：ms
        /// </summary>
        /// <returns></returns>
        public long ElapsedMilliseconds()
        {
            //return (_endTimeTicks - _startTimeTicks) / 10000;
            return _stopwatch.ElapsedMilliseconds;
        }
    }
}
