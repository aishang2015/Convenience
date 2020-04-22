using Microsoft.Extensions.Options;

using MongoDB.Driver;

using System.Collections.Concurrent;

namespace Convience.MongoDB
{
    public class MongoClientContext
    {
        private readonly ConcurrentDictionary<string, MongoClient> ConnectionCache
            = new ConcurrentDictionary<string, MongoClient>();

        private readonly string _mongoConnectionString;

        public MongoClientContext(IOptions<MongoOption> option)
        {
            _mongoConnectionString = option.Value.MongoHost;
        }

        /// <summary>
        /// 取得mongclient
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public MongoClient GetMongoClient(string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = string.Empty;
            }

            if (!ConnectionCache.ContainsKey(connectionString))
            {
                ConnectionCache[connectionString] = InitMongoClient(connectionString);
            }
            return ConnectionCache[connectionString];
        }

        /// <summary>
        /// 初始化mongoclient
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private MongoClient InitMongoClient(string connectionString)
        {
            connectionString = string.IsNullOrEmpty(connectionString) ? _mongoConnectionString : connectionString;
            var client = new MongoClient(connectionString);
            return client;
        }
    }
}
