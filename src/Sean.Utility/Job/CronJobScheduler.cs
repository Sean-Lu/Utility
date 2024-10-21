using System;
using System.Threading;

namespace Sean.Utility.Job;

public delegate void CronJobCallback(CronJobExecutionContext context);

public class CronJobScheduler : IDisposable
{
    public string Name => _name;

    public bool DisallowConcurrentExecution
    {
        get => _disallowConcurrentExecution;
        set => _disallowConcurrentExecution = value;
    }

    public bool IsStarted => _isStarted;
    public bool IsExecuting => _isExecuting;

    private Timer _timer;
    private readonly string _name;
    private readonly CronExpression _cronExpression;
    private readonly CronJobCallback _jobCallback;
    private readonly bool _runWhenStart;
    private bool _disallowConcurrentExecution;
    private bool _isStarted;
    private bool _isExecuting;

    public CronJobScheduler(string name, string cron, CronJobCallback jobCallback, bool runWhenStart, bool disallowConcurrentExecution = false)
    {
        _name = name;
        _cronExpression = new CronExpression(cron);
        _jobCallback = jobCallback;
        _runWhenStart = runWhenStart;
        _disallowConcurrentExecution = disallowConcurrentExecution;
    }

    /// <summary>
    /// 启动定时任务
    /// </summary>
    public void Start()
    {
        if (_isStarted)
        {
            return;
        }

        if (_timer == null)
        {
            // 创建定时器，并设置回调方法和执行间隔
#if NET40
            _timer = new Timer(ExecuteTask, null, _runWhenStart ? TimeSpan.Zero : GetNextExecutionTime(), TimeSpan.FromMilliseconds(-1));
#else
            _timer = new Timer(ExecuteTask, null, _runWhenStart ? TimeSpan.Zero : GetNextExecutionTime(), Timeout.InfiniteTimeSpan);
#endif
        }
        else
        {
            // 重新启动定时器
#if NET40
            _timer?.Change(_runWhenStart ? TimeSpan.Zero : GetNextExecutionTime(), TimeSpan.FromMilliseconds(-1));
#else
            _timer?.Change(_runWhenStart ? TimeSpan.Zero : GetNextExecutionTime(), Timeout.InfiniteTimeSpan);
#endif
        }

        _isStarted = true;
    }

    /// <summary>
    /// 停止定时任务
    /// </summary>
    public void Stop()
    {
        if (!_isStarted)
        {
            return;
        }

        // 停止定时器，但不释放资源
        _timer.Change(Timeout.Infinite, Timeout.Infinite);

        _isStarted = false;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    /// <summary>
    /// 执行任务
    /// </summary>
    /// <param name="state"></param>
    private void ExecuteTask(object state)
    {
        if (_disallowConcurrentExecution && _isExecuting)
        {
            // 禁止并发执行任务
            return;
        }

        // 设置标志，表示任务正在执行
        _isExecuting = true;

        try
        {
            // 重新设置定时器
            var nextExecutionTime = GetNextExecutionTime();
            if (nextExecutionTime > TimeSpan.Zero)
            {
#if NET40
                _timer?.Change(nextExecutionTime, TimeSpan.FromMilliseconds(-1));
#else
                _timer?.Change(nextExecutionTime, Timeout.InfiniteTimeSpan);
#endif
            }

            // 执行任务
            var jobExecutionContext = new CronJobExecutionContext(_name, _timer, nextExecutionTime);
            _jobCallback?.Invoke(jobExecutionContext);
        }
        finally
        {
            // 任务执行完毕，清除标志
            _isExecuting = false;
        }
    }

    /// <summary>
    /// 获取下一次执行时间
    /// </summary>
    /// <returns></returns>
    private TimeSpan GetNextExecutionTime()
    {
        var time = DateTime.Now;
        var nextExecutionTime = _cronExpression.GetNextExecutionTime(time);
        return nextExecutionTime > time ? nextExecutionTime - time : TimeSpan.Zero;
    }
}