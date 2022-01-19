using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;

namespace Sean.Utility.Impls.MQ
{
    public abstract class SimpleLocalMQBase<T>
    {
        protected readonly ISimpleLocalMQ<T> _mq;

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Identity { get; protected set; }

        protected SimpleLocalMQBase(ISimpleLocalMQ<T> mq)
        {
            _mq = mq;
            Identity = GenerateIdentity();
        }

        private string GenerateIdentity()
        {
            return $"{_mq.Name}:{Guid.NewGuid().ToString().Replace("-", string.Empty)}";
        }
    }
}
