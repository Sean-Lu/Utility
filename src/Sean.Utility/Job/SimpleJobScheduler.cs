using System;
using System.Threading;

namespace Sean.Utility.Job;

public delegate void JobCallback(JobExecutionContext context);

/// <summary>
/// 简单定时任务
/// </summary>
public class SimpleJobScheduler : IDisposable
{
    public string Name => _name;

    public bool IsStarted => _isStarted;

    private Timer _timer;
    private readonly string _name;
    private readonly JobCallback _jobCallback;
    private readonly TimeSpan _interval;
    private readonly bool _runWhenStart;
    private readonly bool _disallowConcurrentExecution;
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
    public SimpleJobScheduler(string name, JobCallback jobCallback, TimeSpan interval, bool runWhenStart = false, bool disallowConcurrentExecution = false)
    {
        _name = name;
        _jobCallback = jobCallback;
        _interval = interval;
        _runWhenStart = runWhenStart;
        _disallowConcurrentExecution = disallowConcurrentExecution;
    }

    /// <summary>
    /// 启动定时任务
    /// </summary>
    public virtual void Start()
    {
        if (_isStarted)
        {
            return;
        }

        // 创建定时器，并设置回调方法和执行间隔
        _timer = _runWhenStart
            ? new Timer(ExecuteTask, null, TimeSpan.Zero, _interval)
            : new Timer(ExecuteTask, null, _interval, _interval);

        _isStarted = true;
    }

    /// <summary>
    /// 停止定时任务
    /// </summary>
    public virtual void Stop()
    {
        if (!_isStarted)
        {
            return;
        }

        _timer?.Dispose();

        _isStarted = false;
    }

    public virtual void Dispose()
    {
        _timer?.Dispose();
    }

    /// <summary>
    /// 执行任务
    /// </summary>
    /// <param name="state"></param>
    protected virtual void ExecuteTask(object state)
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
            var jobExecutionContext = new JobExecutionContext
            {
                JobName = _name
            };
            _jobCallback?.Invoke(jobExecutionContext);
        }
        finally
        {
            // 任务执行完毕，清除标志
            _isExecuting = false;
        }
    }
}