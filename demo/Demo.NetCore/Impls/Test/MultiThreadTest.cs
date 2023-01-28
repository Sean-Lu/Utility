using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sean.Utility.Contracts;

namespace Demo.NetCore.Impls.Test
{
    public class MultiThreadTest : ISimpleDo
    {
        private volatile int a;
        private int b;
        private string c;
        private int _testCount = 100000;

        public void Execute()
        {
            // 问：如何保证线程安全？
            // 答：加锁：
            // 互斥锁：lock
            // 监视器：Monitor
            // 读写锁：ReadWriteLock
            // 自由锁：Interlocked（原子操作）
            
            // 堆栈和队列是线程安全的：
            // 说到线程安全，不要一下子就想到加锁，尤其是可能会调用频繁或者是要求高性能的场合。
            // 对于性能要求不高或者同步的对象数量不多的时候，加锁是一个比较简单而且易于实现的选择。比方说.NET提供的一些基础类库，比如线程安全的堆栈和队列，如果使用加锁的方式那么会使性能大打折扣（速度可能会降低好几个数量级），而且如果设计得不好的话还有可能发生死锁。

            // https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/volatile
            // volatile 关键字指示一个字段可以由多个同时执行的线程修改。
            Parallel.For(0, _testCount, i =>
            {
                // 原子操作
                Interlocked.Increment(ref a);

                // volatile并不保证线程安全：复合运算是不支持原子操作的
                //a++;// compound operation is not atomic. 'Interlocked' class can be used instead.
            });

            for (int i = 0; i < _testCount; i++)
            {
                b++;
            }

            Console.WriteLine($"a: {a}");
            Console.WriteLine($"b: {b}");
        }
    }
}
