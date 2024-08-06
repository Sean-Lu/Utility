using System;
using System.Threading;

namespace Sean.Utility.Job;

public delegate void SimpleJobCallback(SimpleJobExecutionContext context);

/// <summary>
/// 简单定时任务
/// </summary>
public class SimpleJobScheduler : IDisposable
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
    private readonly SimpleJobCallback _jobCallback;
    private readonly TimeSpan _delay;
    private readonly TimeSpan _interval;
    private bool _disallowConcurrentExecution;
    private bool _isStarted;
    private bool _isExecuting;

    /// <summary>
    /// 创建简单定时任务
    /// </summary>
    /// <param name="name">任务名称</param>
    /// <param name="jobCallback">执行任务的回调方法</param>
    /// <param name="interval">执行任务间隔时间</param>
    /// <param name="runWhenStart">是否在启动的时候立刻执行任务</param>
    /// <param name="disallowConcurrentExecution">是否禁止并发执行任务</param>
    public SimpleJobScheduler(string name, SimpleJobCallback jobCallback, TimeSpan interval, bool runWhenStart, bool disallowConcurrentExecution = false)
    {
        _name = name;
        _jobCallback = jobCallback;
        _delay = runWhenStart ? TimeSpan.Zero : interval;
        _interval = interval;
        _disallowConcurrentExecution = disallowConcurrentExecution;
    }
    /// <summary>
    /// 创建简单定时任务
    /// </summary>
    /// <param name="name">任务名称</param>
    /// <param name="jobCallback">执行任务的回调方法</param>
    /// <param name="interval">执行任务间隔时间（单位：毫秒）</param>
    /// <param name="runWhenStart">是否在启动的时候立刻执行任务</param>
    /// <param name="disallowConcurrentExecution">是否禁止并发执行任务</param>
    public SimpleJobScheduler(string name, SimpleJobCallback jobCallback, int interval, bool runWhenStart, bool disallowConcurrentExecution = false)
    {
        _name = name;
        _jobCallback = jobCallback;
        _delay = runWhenStart ? TimeSpan.Zero : TimeSpan.FromMilliseconds(interval);
        _interval = TimeSpan.FromMilliseconds(interval);
        _disallowConcurrentExecution = disallowConcurrentExecution;
    }
    /// <summary>
    /// 创建简单定时任务
    /// </summary>
    /// <param name="name">任务名称</param>
    /// <param name="jobCallback">执行任务的回调方法</param>
    /// <param name="delay">延迟执行任务的时间</param>
    /// <param name="interval">执行任务间隔时间</param>
    /// <param name="disallowConcurrentExecution">是否禁止并发执行任务</param>
    public SimpleJobScheduler(string name, SimpleJobCallback jobCallback, TimeSpan delay, TimeSpan interval, bool disallowConcurrentExecution = false)
    {
        _name = name;
        _jobCallback = jobCallback;
        _delay = delay;
        _interval = interval;
        _disallowConcurrentExecution = disallowConcurrentExecution;
    }
    /// <summary>
    /// 创建简单定时任务
    /// </summary>
    /// <param name="name">任务名称</param>
    /// <param name="jobCallback">执行任务的回调方法</param>
    /// <param name="delay">延迟执行任务的时间（单位：毫秒）</param>
    /// <param name="interval">执行任务间隔时间（单位：毫秒）</param>
    /// <param name="disallowConcurrentExecution">是否禁止并发执行任务</param>
    public SimpleJobScheduler(string name, SimpleJobCallback jobCallback, int delay, int interval, bool disallowConcurrentExecution = false)
    {
        _name = name;
        _jobCallback = jobCallback;
        _delay = TimeSpan.FromMilliseconds(delay);
        _interval = TimeSpan.FromMilliseconds(interval);
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
            _timer = new Timer(ExecuteTask, null, _delay, _interval);
        }
        else
        {
            // 重新启动定时器
            _timer.Change(_delay, _interval);
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
            // 执行任务
            var jobExecutionContext = new SimpleJobExecutionContext(_name, _timer, _delay, _interval);
            _jobCallback?.Invoke(jobExecutionContext);
        }
        finally
        {
            // 任务执行完毕，清除标志
            _isExecuting = false;
        }
    }
}