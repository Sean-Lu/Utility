using System;
using System.Diagnostics;
using Sean.Utility.Contracts;

namespace Sean.Utility.Common
{
    /// <summary>
    /// 计时器（<see cref="DateTime"/>）
    /// </summary>
    public class TimeWatchWrapper : ITimeWatch
    {
        private long _startTicks;
        private long _endTime;

        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            _startTicks = DateTime.Now.Ticks;
        }

        /// <summary>
        /// 重新开始计时
        /// </summary>
        public void Restart()
        {
            _startTicks = DateTime.Now.Ticks;
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop()
        {
            _endTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// 返回耗时，单位：ms
        /// </summary>
        /// <returns></returns>
        public long ElapsedMilliseconds()
        {
            return (_endTime - _startTicks) / 10000;
        }
    }

    /// <summary>
    /// 计时器（<see cref="Stopwatch"/>）
    /// </summary>
    public class TimeWatch2Wrapper : ITimeWatch
    {
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// 计时器
        /// </summary>
        public TimeWatch2Wrapper()
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
            return _stopwatch.ElapsedMilliseconds;
        }
    }
}
