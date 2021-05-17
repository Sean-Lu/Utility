using Sean.Utility.Enums;

namespace Sean.Utility.Contracts
{
    public interface ISimpleQueueOptions
    {
        /// <summary>
        /// 队列触发器类型
        /// </summary>
        QueueTriggerType QueueTriggerType { get; }

        /// <summary>
        /// QueueTriggerType.Timer => 定时器时间间隔（以毫秒为单位）
        /// </summary>
        double TimerInterval { get; }

        /// <summary>
        /// QueueTriggerType.Count => 数量上限
        /// </summary>
        int CountLimit { get; }

        /// <summary>
        /// 最大重试消费的次数（当消费失败时重新丢入队列等待下次消费）。注：当值小于0时会一直尝试重新消费，直到消费成功为止。
        /// </summary>
        int MaxReconsumeCount { get; }

        /// <summary>
        /// 在重新消费之前的等待时间（以毫秒为单位），默认值为3秒
        /// </summary>
        int ReconsumeSleepTime { get; }

        /// <summary>
        /// 是否开启异步消费，默认为false（同步消费）。建议启用异步消费，可以提高消费速度，也不会阻塞生产者。
        /// </summary>
        bool ConsumeAsync { get; }

        /// <summary>
        /// 是否开启数据持久化
        /// </summary>
        //bool DataPersistent { get; }
    }
}