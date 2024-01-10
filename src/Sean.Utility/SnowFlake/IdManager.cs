using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sean.Utility.Contracts;

namespace Sean.Utility.SnowFlake
{
    public class IdManager : IIdManager
    {
        /// <summary>
        /// ��������ID(0~31)
        /// </summary>
        public long DatacenterId { get; set; }
        /// <summary>
        /// ��������ID(0~31)
        /// </summary>
        public long WorkerId { get; set; }

        private static readonly ConcurrentDictionary<long, IdWorker> _idWorkerCache;

        /// <summary>
        /// �Ƿ��ʼ��������
        /// </summary>
        protected bool IsConfigInitialized { get; }

        static IdManager()
        {
            _idWorkerCache = new ConcurrentDictionary<long, IdWorker>();
        }

        public IdManager()
        {
            #region �ӻ���������ȡ����
            var enviVar = Environment.GetEnvironmentVariable("DatacenterAndWorkerId");// ��������ͨ����|���ָ�DatacenterId��WorkerId
            if (!string.IsNullOrWhiteSpace(enviVar))
            {
                var idList = enviVar.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(c =>
                {
                    if (int.TryParse(c, out var result) && result >= 0)
                    {
                        return result;
                    }
                    return 0;
                }).ToList();
                if (idList.Any())
                {
                    IsConfigInitialized = true;
                    DatacenterId = idList[0];
                    if (idList.Count > 1)
                    {
                        WorkerId = idList[1];
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// �����һ��ID (�̰߳�ȫ) 
        /// </summary>
        /// <returns>ȫ��Ψһ��id</returns>
        public virtual long NextId()
        {
            var idWorker = _idWorkerCache.GetOrAdd(DatacenterId, key => new IdWorker(DatacenterId, WorkerId));
            return idWorker.NextId();
        }
        /// <summary>
        /// �����һ��ID (�̰߳�ȫ) 
        /// </summary>
        /// <returns>ȫ��Ψһ��id</returns>
        public virtual List<long> NextIds(int count)
        {
            var idWorker = _idWorkerCache.GetOrAdd(DatacenterId, key => new IdWorker(DatacenterId, WorkerId));
            var list = new List<long>();
            for (var i = 0; i < count; i++)
            {
                list.Add(idWorker.NextId());
            }
            return list;
        }
    }
    public class IdManager<T> : IdManager, IIdManager<T> where T : class
    {
        public IdManager()
        {
            if (DatacenterId == 0 && !IsConfigInitialized)
            {
                DatacenterId = Math.Abs(typeof(T).FullName.GetHashCode()) % 32;
            }
        }
    }
}
