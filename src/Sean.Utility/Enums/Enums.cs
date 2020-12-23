namespace Sean.Utility.Enums
{
    /// <summary>
    /// 配置文件类型
    /// </summary>
    public enum ConfigFileType
    {
        /// <summary>
        /// 自动匹配，默认匹配优先级别：AppConfig > WebConfig
        /// </summary>
        Auto,
        /// <summary>
        /// App.config（Winform）
        /// </summary>
        AppConfig,
        /// <summary>
        /// Web.config（WebForm）
        /// </summary>
        WebConfig
    }

    /// <summary>
    /// 时间戳类型
    /// </summary>
    public enum TimestampType
    {
        /// <summary>
        /// JavaScript时间戳：是指格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总毫秒数。
        /// </summary>
        JavaScript,
        /// <summary>
        /// Unix时间戳：是指格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总秒数。
        /// </summary>
        Unix
    }

    /// <summary>
    /// 路径搜索级别（遍历）
    /// </summary>
    public enum PathSearchLevel
    {
        /// <summary>
        /// 遍历1层（当前层）
        /// </summary>
        One,
        /// <summary>
        /// 遍历全部
        /// </summary>
        All
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
