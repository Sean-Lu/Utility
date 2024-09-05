using System;
using Demo.Framework.Contracts;
using Sean.Utility;
using Sean.Utility.Contracts;
using Sean.Utility.Extensions;

namespace Demo.Framework.Impls.Test
{
    /// <summary>
    /// 时间戳测试
    /// </summary>
    public class TimestampTest : ISimpleDo
    {
        public void Execute()
        {
            var now = DateTime.Now;
            Console.WriteLine($"当前时间：{now.ToLongDateTimeWithTimezone()}");

            var timestamp = DateTimeHelper.GetTimestamp(now);
            Console.WriteLine($"当前时间戳：{timestamp}");

            var time = DateTimeHelper.GetDateTime(timestamp);
            //Console.WriteLine($"当前时间：{JsonConvert.SerializeObject(time)}");
            Console.WriteLine($"当前时间：{time.ToLongDateTimeWithTimezone()}");
        }
    }
}
