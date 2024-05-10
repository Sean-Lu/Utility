using System;
using System.Threading;

namespace Sean.Utility.Job
{
    public delegate void TaskCallback();

    public class SimpleTaskScheduler : IDisposable
    {
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public TaskCallback Callback
        {
            get => _callback;
            set => _callback = value;
        }
        public TimeSpan Interval
        {
            get => _interval;
            set => _interval = value;
        }
        public bool StartNow
        {
            get => _startNow;
            set => _startNow = value;
        }

        private Timer _timer;
        private string _name;
        private TaskCallback _callback;
        private TimeSpan _interval;
        private bool _startNow;

        public SimpleTaskScheduler()
        {

        }
        public SimpleTaskScheduler(string name, TaskCallback callback, TimeSpan interval, bool startNow = true)
        {
            _name = name;
            _callback = callback;
            _interval = interval;
            _startNow = startNow;
        }

        /// <summary>
        /// 启动定时任务
        /// </summary>
        public virtual void Start()
        {
            // 创建定时器，并设置回调方法和执行间隔
            _timer = _startNow
                ? new Timer(ExecuteTask, null, TimeSpan.Zero, _interval)
                : new Timer(ExecuteTask, null, _interval, _interval);
        }

        /// <summary>
        /// 停止定时任务
        /// </summary>
        public virtual void Stop()
        {
            _timer.Dispose();
        }

        public virtual void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="state"></param>
        protected virtual void ExecuteTask(object state)
        {
            _callback?.Invoke();
        }
    }
}
