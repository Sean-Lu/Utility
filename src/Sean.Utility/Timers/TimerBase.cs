using System.Timers;

namespace Sean.Utility.Timers
{
    /// <summary>
    /// 定时器基类
    /// </summary>
    public abstract class TimerBase
    {
        //在C#里关于定时器类有3个
        //1. 定义在System.Windows.Forms里
        //2. 定义在System.Threading.Timer类里
        //3. 定义在System.Timers.Timer类里
        //System.Windows.Forms.Timer是应用于WinForm中的，它是通过Windows消息机制实现的，类似于VB或Delphi中的Timer控件，内部使用API SetTimer实现的。它的主要缺点是计时不精确，而且必须有消息循环，Console Application(控制台应用程序)无法使用。
        //System.Timers.Timer和System.Threading.Timer非常类似，它们是通过.NET Thread Pool实现的，轻量，计时精确，对应用程序、消息没有特别的要求。System.Timers.Timer还可以应用于WinForm，完全取代上面的Timer控件。它们的缺点是不支持直接的拖放，需要手工编码。

        private readonly Timer _timer;

        /// <summary>
        /// 是否正在执行中
        /// </summary>
        private bool _isRunning;
        /// <summary>
        /// 是否禁止并发执行
        /// </summary>
        private readonly bool _disallowConcurrentExecution;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interval">触发<see cref="Elapsed"/>执行的时间间隔（以毫秒为单位）</param>
        /// <param name="autoReset"><see cref="Elapsed"/>只执行1次（false）或重复执行（true）</param>
        /// <param name="disallowConcurrentExecution">是否禁止并发执行</param>
        protected TimerBase(double interval, bool autoReset = true, bool disallowConcurrentExecution = true)
        {
            _timer = new Timer
            {
                Interval = interval,
                AutoReset = autoReset
            };
            _timer.Elapsed += _timer_Elapsed;

            _disallowConcurrentExecution = disallowConcurrentExecution;
        }

        /// <summary>
        /// 开始定时器
        /// </summary>
        public virtual void Start()
        {
            _timer.Start();
        }

        /// <summary>
        /// 停止定时器
        /// </summary>
        public virtual void Stop()
        {
            _timer.Stop();
        }

        /// <summary>
        /// 达到时间间隔时触发执行的方法
        /// </summary>
        protected abstract void Elapsed();

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_isRunning && _disallowConcurrentExecution)
            {
                return;
            }

            _isRunning = true;

            try
            {
                Elapsed();
            }
            finally
            {
                _isRunning = false;
            }
        }
    }
}
