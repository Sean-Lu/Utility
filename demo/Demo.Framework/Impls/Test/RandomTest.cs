using System;
using Demo.Framework.Contracts;
using Sean.Utility;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;

namespace Demo.Framework.Impls.Test
{
    internal class RandomTest : ISimpleDo
    {
        public void Execute()
        {
            // 随机字符串：纯数字
            Console.WriteLine(RandomHelper.NextString(4, RandomStringType.Number));
            Console.WriteLine(RandomHelper.NextString(4, RandomStringType.Number));
            Console.WriteLine(RandomHelper.NextString(4, RandomStringType.Number));
            Console.WriteLine("===========================");

            // 随机字符串：数字 + 大写字母
            Console.WriteLine(RandomHelper.NextString(4, RandomStringType.Number | RandomStringType.AbcUpper));
            Console.WriteLine(RandomHelper.NextString(4, RandomStringType.Number | RandomStringType.AbcUpper));
            Console.WriteLine(RandomHelper.NextString(4, RandomStringType.Number | RandomStringType.AbcUpper));
            Console.WriteLine("===========================");

            // 随机字符串：数字 + 小写字母
            Console.WriteLine(RandomHelper.NextString(4, RandomStringType.Number | RandomStringType.AbcLower));
            Console.WriteLine(RandomHelper.NextString(4, RandomStringType.Number | RandomStringType.AbcLower));
            Console.WriteLine(RandomHelper.NextString(4, RandomStringType.Number | RandomStringType.AbcLower));
            Console.WriteLine("===========================");
        }
    }
}
