namespace Sean.Utility.Contracts
{
    public interface ISimpleService
    {
        bool IsStarted { get; }

        void Start();
        void Stop();
    }

    public interface ISimpleService<in T>
    {
        bool IsStarted { get; }

        void Start(T data);
        void Stop();
    }

    public interface ISimpleService<in T, in T2>
    {
        bool IsStarted { get; }

        void Start(T data);
        void Stop(T2 data);
    }
}
