namespace Sean.Utility.Contracts
{
    /// <summary>
    /// 计时器
    /// </summary>
    public interface ITimeWatch
    {
        /// <summary>
        /// 开始计时
        /// </summary>
        void Start();

        /// <summary>
        /// 重新开始计时
        /// </summary>
        void Restart();

        /// <summary>
        /// 停止计时
        /// </summary>
        void Stop();

        /// <summary>
        /// 返回耗时，单位：ms
        /// </summary>
        /// <returns></returns>
        long ElapsedMilliseconds();
    }
}
