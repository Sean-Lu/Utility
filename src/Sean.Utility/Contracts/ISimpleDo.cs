namespace Sean.Utility.Contracts
{
    /// <summary>
    /// Do something simply
    /// </summary>
    public interface ISimpleDo
    {
        /// <summary>
        /// Begin execution
        /// </summary>
        void Execute();
    }

    /// <summary>
    /// Do something simply
    /// </summary>
    public interface ISimpleDo<in T>
    {
        /// <summary>
        /// Begin execution
        /// </summary>
        /// <param name="data"></param>
        void Execute(T data);
    }
}
