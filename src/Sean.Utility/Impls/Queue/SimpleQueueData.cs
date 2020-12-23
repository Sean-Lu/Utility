using System;

namespace Sean.Utility.Impls.Queue
{
    public class SimpleQueueData<T>
    {
        public SimpleQueueData(T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            Data = data;
        }

        public T Data { get; set; }

        /// <summary>
        /// 重新消费的次数
        /// </summary>
        public int ReconsumeCount { get; internal set; }
    }
}
