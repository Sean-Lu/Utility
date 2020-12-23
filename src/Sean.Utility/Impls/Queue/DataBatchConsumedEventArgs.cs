using System;
using System.Collections.Generic;
using Sean.Utility.Enums;

namespace Sean.Utility.Impls.Queue
{
    public class DataBatchConsumedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 队列中的数据
        /// </summary>
        public IEnumerable<SimpleQueueData<T>> QueueData { get; }
        /// <summary>
        /// 触发器类型
        /// </summary>
        public QueueTriggerType TriggerType { get; }
        /// <summary>
        /// 是否需要重新消费
        /// </summary>
        public bool ShouldReconsume { get; set; }

        public DataBatchConsumedEventArgs(IEnumerable<SimpleQueueData<T>> queueData, QueueTriggerType triggerType)
        {
            QueueData = queueData;
            TriggerType = triggerType;
        }
    }
}
