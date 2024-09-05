using System;

namespace Sean.Utility.Enums;

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

/// <summary>
/// 消息队列类型
/// </summary>
public enum MQType
{
    /// <summary>
    /// 点对点（point to point）：不可重复消费
    /// </summary>
    Queue,
    /// <summary>
    /// 发布/订阅（publish/subscribe）：可以重复消费
    /// </summary>
    Topic
}