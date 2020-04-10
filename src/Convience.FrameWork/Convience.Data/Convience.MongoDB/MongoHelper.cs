using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.MongoDB
{
    public class MongoHelper
    {
        private IMongoDatabase _dataBase;

        private MongoClient _client;

        public MongoHelper(MongoClientHelper clienthelper, IOptions<MongoOption> option)
        {
            _client = clienthelper.GetMongoClient();
            _dataBase = _client.GetDatabase(option.Value.DefaultDataBase);
        }

        public void SwitchDataBase(string dataBaseName)
        {
            _dataBase = _client.GetDatabase(dataBaseName);
        }

        private IMongoCollection<T> GetCollection<T>()
        {
            return _dataBase.GetCollection<T>(typeof(T).Name);
        }


        #region 添加数据

        /// <summary>
        /// 添加一条数据
        /// </summary>
        public void Add<T>(T t)
        {
            GetCollection<T>().InsertOne(t);
        }

        /// <summary>
        /// 添加一条数据
        /// </summary>
        public async Task AddAsync<T>(T t)
        {
            await GetCollection<T>().InsertOneAsync(t);
        }

        /// <summary>
        /// 添加多条数据
        /// </summary>
        public void Add<T>(List<T> tList)
        {
            GetCollection<T>().InsertMany(tList);
        }

        /// <summary>
        /// 添加多条数据
        /// </summary>
        public async Task AddAsync<T>(List<T> tList)
        {
            await GetCollection<T>().InsertManyAsync(tList);
        }

        #endregion

        #region 更新数据

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update<T>(T t)
        {
            // 条件
            var id = t.GetType().GetProperty("_id").GetValue(t).ToString();
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));

            // 要修改的字段
            var list = new List<UpdateDefinition<T>>();
            foreach (var item in t.GetType().GetProperties())
            {
                if (item.Name.ToLower() == "id") continue;
                list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(t)));
            }
            var updateDefinition = Builders<T>.Update.Combine(list);
            GetCollection<T>().UpdateOne(filter, updateDefinition);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public async Task UpdateAsync<T>(T t)
        {
            // 条件
            var id = t.GetType().GetProperty("Id").GetValue(t).ToString();
            var filter = Builders<T>.Filter.Eq("_id", id);

            // 要修改的字段
            var list = new List<UpdateDefinition<T>>();
            foreach (var item in t.GetType().GetProperties())
            {
                if (item.Name.ToLower() == "id") continue;
                list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(t)));
            }
            var updateDefinition = Builders<T>.Update.Combine(list);
            await GetCollection<T>().UpdateOneAsync(filter, updateDefinition);
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        public void Update<T>(FilterDefinition<T> filter, Dictionary<string, string> updateDic)
        {
            // 要修改的字段
            var list = new List<UpdateDefinition<T>>();
            var properties = typeof(T).GetProperties().Select(p => p.Name);
            foreach (var kvp in updateDic)
            {
                if (properties.Contains(kvp.Key))
                {
                    list.Add(Builders<T>.Update.Set(kvp.Key, kvp.Value));
                }
            }
            var updateDefinition = Builders<T>.Update.Combine(list);
            GetCollection<T>().UpdateMany(filter, updateDefinition);
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        public async Task UpdateAsync<T>(FilterDefinition<T> filter, Dictionary<string, string> updateDic)
        {
            // 要修改的字段
            var list = new List<UpdateDefinition<T>>();
            var properties = typeof(T).GetProperties().Select(p => p.Name);
            foreach (var kvp in updateDic)
            {
                if (properties.Contains(kvp.Key))
                {
                    list.Add(Builders<T>.Update.Set(kvp.Key, kvp.Value));
                }
            }
            var updateDefinition = Builders<T>.Update.Combine(list);
            await GetCollection<T>().UpdateManyAsync(filter, updateDefinition);
        }

        #endregion

        #region 删除数据

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        public void Delete<T>(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            GetCollection<T>().DeleteOne(filter);
        }

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        public async Task DeleteAsync<T>(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            await GetCollection<T>().DeleteOneAsync(filter);
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public void Delete<T>(FilterDefinition<T> filter)
        {
            GetCollection<T>().DeleteMany(filter);
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public async Task DeleteAsync<T>(FilterDefinition<T> filter)
        {
            await GetCollection<T>().DeleteManyAsync(filter);
        }


        #endregion

        #region 查询数据

        /// <summary>
        /// 根据id查询数据
        /// </summary>
        public T Get<T>(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            return GetCollection<T>().Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// 根据id查询数据
        /// </summary>
        public async Task<T> GetAsync<T>(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            var finder = await GetCollection<T>().FindAsync(filter);
            return finder.FirstOrDefault();
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        public List<T> Get<T>(FilterDefinition<T> filter)
        {
            return GetCollection<T>().Find(filter).ToList();
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        public async Task<List<T>> GetAsync<T>(FilterDefinition<T> filter)
        {
            var finder = await GetCollection<T>().FindAsync(filter);
            return finder.ToList();
        }

        /// <summary>
        /// 查询分页数据
        /// </summary>
        public List<T> GetByPage<T>(FilterDefinition<T> filter, int pageIndex, int pageSize, SortDefinition<T> sort = null)
        {
            var skip = pageSize * (pageIndex - 1);
            var finder = GetCollection<T>().Find(filter).Sort(sort);
            return finder.Skip(skip).Limit(pageSize).ToList();
        }

        /// <summary>
        /// 查询分页数据
        /// </summary>
        public async Task<List<T>> GetByPageAsync<T>(FilterDefinition<T> filter, int pageIndex, int pageSize, SortDefinition<T> sort = null)
        {
            var skip = pageSize * (pageIndex - 1);
            var finder = GetCollection<T>().Find(filter).Sort(sort);
            return await finder.Skip(skip).Limit(pageSize).ToListAsync();
        }

        #endregion

    }
}
