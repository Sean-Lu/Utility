using Sean.Utility;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;
using Sean.Utility.Extensions;
using Sean.Utility.SnowFlake;
using System;
using System.Diagnostics;

namespace Demo.NetCore.Impls.Test
{
    internal class SnowFlakeTest : ISimpleDo
    {
        public void Execute()
        {
            //Console.WriteLine(DateTimeHelper.GetTimestamp(DateTime.Today));
            //Console.WriteLine(DateTimeHelper.GetDateTime(IdWorker.StartTimestamp, TimestampType.TotalMilliseconds).ToLongDateTimeWithTimezone());

            var idManager = new IdManager<Program>();
            var count = 10000;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //DelegateHelper.Repeat(() =>
            //{
            //    var id = idManager.NextId();
            //    Console.WriteLine(id);
            //    return false;
            //}, count);

            var ids = idManager.NextIds(count);
            ids.ForEach(Console.WriteLine);

            stopwatch.Stop();
            Console.WriteLine($"生成{count}个id，耗时：{stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
