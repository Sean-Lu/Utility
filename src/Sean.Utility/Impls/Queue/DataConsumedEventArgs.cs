using System;
using Sean.Utility.Enums;

namespace Sean.Utility.Impls.Queue
{
    public class DataConsumedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 队列中的数据
        /// </summary>
        public SimpleQueueData<T> QueueData { get; }
        /// <summary>
        /// 触发器类型
        /// </summary>
        public QueueTriggerType TriggerType { get; }
        /// <summary>
        /// 是否需要重新消费
        /// </summary>
        public bool ShouldReconsume { get; set; }

        public DataConsumedEventArgs(SimpleQueueData<T> queueData, QueueTriggerType triggerType)
        {
            QueueData = queueData;
            TriggerType = triggerType;
        }
    }
}
