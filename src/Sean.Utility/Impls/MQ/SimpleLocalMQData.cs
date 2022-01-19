using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sean.Utility.Impls.MQ
{
    public class SimpleLocalMQData<T>
    {
        public SimpleLocalMQData(T data)
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
