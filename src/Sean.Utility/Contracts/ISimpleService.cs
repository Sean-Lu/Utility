namespace Sean.Utility.Contracts
{
    public interface ISimpleService
    {
        void Start();
        void Stop();
    }

    public interface ISimpleService<in T>
    {
        void Start(T data);
        void Stop();
    }

    public interface ISimpleService<in T, in T2>
    {
        void Start(T data);
        void Stop(T2 data);
    }
}
