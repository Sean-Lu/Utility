using System;

namespace Sean.Utility.Enums
{
    /// <summary>
    /// 队列触发器类型
    /// </summary>
    [Flags]
    public enum QueueTriggerType
    {
        /// <summary>
        /// 立刻触发
        /// </summary>
        Immediate = 0,
        /// <summary>
        /// 数量达到满足条件时触发
        /// </summary>
        Count = 1,
        /// <summary>
        /// 定时触发
        /// </summary>
        Timer = 2,
        /// <summary>
        /// 手动触发
        /// </summary>
        Manual = 4
    }
}
