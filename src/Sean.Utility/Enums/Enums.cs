﻿namespace Sean.Utility.Enums
{
    /// <summary>
    /// 时间戳类型
    /// </summary>
    public enum TimestampType
    {
        /// <summary>
        /// 格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总毫秒数。
        /// </summary>
        TotalMilliseconds,
        /// <summary>
        /// 格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总秒数。
        /// </summary>
        TotalSeconds
    }

    /// <summary>
    /// 显示器状态
    /// </summary>
    public enum MonitorState
    {
        /// <summary>
        /// 打开
        /// </summary>
        On,
        /// <summary>
        /// 关闭
        /// </summary>
        Off,
        /// <summary>
        /// 省电
        /// </summary>
        StandBy
    }

    /// <summary>
    /// Code128条码类型
    /// </summary>
    public enum Code128Type
    {
        Code128A,
        Code128B,
        Code128C,
        EAN128
    }
}
