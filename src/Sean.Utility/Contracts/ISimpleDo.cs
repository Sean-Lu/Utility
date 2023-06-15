namespace Sean.Utility.Contracts
{
    public interface ISimpleDo
    {
        void Execute();
    }

    public interface ISimpleDo<in T>
    {
        void Execute(T data);
    }
}
