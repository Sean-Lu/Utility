using Sean.Utility.Enums;

namespace Sean.Utility.Impls.Queue
{
    /// <summary>
    /// 队列配置
    /// </summary>
    public class SimpleQueueOptions
    {
        /// <summary>
        /// 队列触发器类型
        /// </summary>
        public QueueTriggerType QueueTriggerType { get; set; }

        /// <summary>
        /// QueueTriggerType.Timer => 定时器时间间隔（以毫秒为单位）
        /// </summary>
        public double TimerInterval { get; set; }

        /// <summary>
        /// QueueTriggerType.Count => 数量上限
        /// </summary>
        public int CountLimit { get; set; }

        /// <summary>
        /// 最大重试消费的次数（当消费失败时重新丢入队列等待下次消费）。注：当值小于0时会一直尝试重新消费，直到消费成功为止。
        /// </summary>
        public int MaxReconsumeCount { get; set; }
        /// <summary>
        /// 在重新消费之前的等待时间（以毫秒为单位），默认值为3秒
        /// </summary>
        public int ReconsumeSleepTime { get; set; } = 3000;

        /// <summary>
        /// 是否开启异步消费，默认为false（同步消费）。建议启用异步消费，可以提高消费速度，也不会阻塞生产者。
        /// </summary>
        public bool ConsumeAsync { get; set; }

        /// <summary>
        /// 是否开启数据持久化
        /// </summary>
        //public bool DataPersistent { get; set; }
    }
}
