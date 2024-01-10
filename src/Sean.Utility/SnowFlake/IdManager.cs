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
        /// 数据中心ID(0~31)
        /// </summary>
        public long DatacenterId { get; set; }
        /// <summary>
        /// 工作机器ID(0~31)
        /// </summary>
        public long WorkerId { get; set; }

        private static readonly ConcurrentDictionary<long, IdWorker> _idWorkerCache;

        /// <summary>
        /// 是否初始化过配置
        /// </summary>
        protected bool IsConfigInitialized { get; }

        static IdManager()
        {
            _idWorkerCache = new ConcurrentDictionary<long, IdWorker>();
        }

        public IdManager()
        {
            #region 从环境变量读取配置
            var enviVar = Environment.GetEnvironmentVariable("DatacenterAndWorkerId");// 环境变量通过“|”分隔DatacenterId和WorkerId
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
        /// 获得下一个ID (线程安全) 
        /// </summary>
        /// <returns>全局唯一的id</returns>
        public virtual long NextId()
        {
            var idWorker = _idWorkerCache.GetOrAdd(DatacenterId, key => new IdWorker(DatacenterId, WorkerId));
            return idWorker.NextId();
        }
        /// <summary>
        /// 获得下一个ID (线程安全) 
        /// </summary>
        /// <returns>全局唯一的id</returns>
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
