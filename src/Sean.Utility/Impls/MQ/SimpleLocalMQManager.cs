using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sean.Utility.Impls.MQ
{
    public class SimpleLocalMQManager
    {
        public static readonly ConcurrentDictionary<string, ICollection> QueueCollections = new ConcurrentDictionary<string, ICollection>();
    }
}
