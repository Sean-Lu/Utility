using System;
using System.Threading;

namespace Sean.Utility.Job;

public class SimpleJobExecutionContext
{
    public SimpleJobExecutionContext(string jobName, Timer timer, TimeSpan delay, TimeSpan interval)
    {
        JobName = jobName;
        Timer = timer;
        Delay = delay;
        Interval = interval;
    }

    public string JobName { get; }
    public Timer Timer { get; }
    public TimeSpan Delay { get; }
    public TimeSpan Interval { get; }
}