using System.Collections.Generic;

namespace Sean.Utility.Contracts
{
    public interface IIdManager
    {
        /// <summary>
        /// ��������ID(0~31)
        /// </summary>
        long DatacenterId { get; set; }
        /// <summary>
        /// ��������ID(0~31)
        /// </summary>
        long WorkerId { get; set; }

        /// <summary>
        /// �����һ��ID (�̰߳�ȫ) 
        /// </summary>
        /// <returns>ȫ��Ψһ��id</returns>
        long NextId();
        /// <summary>
        /// �����һ��ID (�̰߳�ȫ) 
        /// </summary>
        /// <returns>ȫ��Ψһ��id</returns>
        List<long> NextIds(int count);
    }
    public interface IIdManager<T> : IIdManager where T : class
    {

    }
}
