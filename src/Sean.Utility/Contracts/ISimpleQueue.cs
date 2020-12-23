using System.Collections.Generic;
using Sean.Utility.Enums;
using Sean.Utility.Impls.Queue;

namespace Sean.Utility.Contracts
{
    /// <summary>
    /// Simple implementation of ConcurrentQueue&lt;T&gt;. Note: See enum <see cref="QueueTriggerType"/> for the supported queue trigger types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISimpleQueue<T>
    {
        /// <summary>
        /// 获取当前队列中数据的数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 当前队列是否为空
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 队列配置
        /// </summary>
        SimpleQueueOptions Options { get; }

        /// <summary>
        /// 将单个数据插入队列
        /// </summary>
        /// <param name="item"></param>
        void Enqueue(T item);

        /// <summary>
        /// 将单个数据插入队列
        /// </summary>
        /// <param name="item"></param>
        void Enqueue(SimpleQueueData<T> item);

        /// <summary>
        /// 将批量数据插入队列
        /// </summary>
        /// <param name="items"></param>
        void Enqueue(IEnumerable<T> items);

        /// <summary>
        /// 将批量数据插入队列
        /// </summary>
        /// <param name="items"></param>
        void Enqueue(IEnumerable<SimpleQueueData<T>> items);

        /// <summary>
        /// 手动执行1次消费（必须先设置允许手动触发消费：<see cref="SimpleQueueOptions.QueueTriggerType"/> |= QueueTriggerType.Manual;）
        /// </summary>
        /// <param name="count">消费的数量（小于0：消费全部数据，大于0：消费部分数据）</param>
        /// <returns>本次队列中实际被消费的数量</returns>
        int ExecuteConsume(int count);
    }
}
