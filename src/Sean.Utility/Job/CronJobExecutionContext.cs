using System;
using System.Threading;

namespace Sean.Utility.Job;

public class CronJobExecutionContext
{
    public CronJobExecutionContext(string jobName, Timer timer, TimeSpan nextExecutionTime)
    {
        JobName = jobName;
        Timer = timer;
        NextExecutionTime = nextExecutionTime;
    }

    public string JobName { get; }
    public Timer Timer { get; }
    public TimeSpan NextExecutionTime { get; }
}