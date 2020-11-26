using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Convience.MemoryMQ
{
    public class Good
    {

    }

    public class MemeoryMQProvider
    {
        // 所有队列
        public static Dictionary<string, ConcurrentQueue<Good>> QueueDic =
            new Dictionary<string, ConcurrentQueue<Good>>();

        // 取得队列
        public ConcurrentQueue<T> GetQueue<T>(string name) where T : IMemeoryMessage
        {
            if (!QueueDic.ContainsKey(name))
            {
                throw new Exception("Not found that key.");
            }

            var queue = QueueDic[name];
            if (queue == null)
            {
                throw new Exception("Not found any queue.");
            }

            if (!(queue is ConcurrentQueue<T>))
            {
                throw new Exception("ConcurrentQueue type is not fit.");
            }

            return queue as ConcurrentQueue<T>;
        }
    }
}
