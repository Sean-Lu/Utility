namespace Sean.Utility.Impls.MQ
{
    public class SimpleLocalMQOptions
    {
        /// <summary>
        /// 最大重新消费的次数
        /// </summary>
        public int MaxReconsumeTimes { get; set; }
        /// <summary>
        /// 重新消费的等待时间（单位：毫秒）
        /// </summary>
        public int ReconsumeWaitTime { get; set; }
        /// <summary>
        /// 并行消费（多消费者）
        /// </summary>
        public bool ConcurrentConsume { get; set; }
    }
}
