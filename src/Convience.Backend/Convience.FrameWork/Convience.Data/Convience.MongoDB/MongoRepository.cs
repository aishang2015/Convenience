using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.MongoDB
{
    public class MongoRepository
    {
        private IMongoDatabase _dataBase;

        private MongoClient _client;

        private GridFSBucket _bucket;

        public MongoRepository(MongoClientContext clienthelper, IOptions<MongoOption> option)
        {
            _client = clienthelper.GetMongoClient();
            _dataBase = _client.GetDatabase(option.Value.DefaultDataBase);
            _bucket = new GridFSBucket(_dataBase);
        }

        public void SwitchDataBase(string dataBaseName)
        {
            _dataBase = _client.GetDatabase(dataBaseName);
        }

        private IMongoCollection<T> GetCollection<T>()
        {
            return _dataBase.GetCollection<T>(typeof(T).Name);
        }

        #region 索引操作

        /// <summary>
        /// 添加索引
        /// </summary>
        public async Task AddIndex<T>(FieldDefinition<T> field)
        {
            var indexOptions = new CreateIndexOptions();
            var indexKeys = Builders<T>.IndexKeys.Ascending(field);
            var indexModel = new CreateIndexModel<T>(indexKeys, indexOptions);
            await GetCollection<T>().Indexes.CreateOneAsync(indexModel);
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        public async Task AddIndex<T>(IEnumerable<FieldDefinition<T>> fields)
        {
            var indexModels = new List<CreateIndexModel<T>>();
            foreach (var field in fields)
            {
                var indexOptions = new CreateIndexOptions();
                var indexKeys = Builders<T>.IndexKeys.Ascending(field);
                indexModels.Add(new CreateIndexModel<T>(indexKeys, indexOptions));
            }
            await GetCollection<T>().Indexes.CreateManyAsync(indexModels);
        }

        #endregion

        #region GridFS处理

        /// <summary>
        /// 下载文件
        /// </summary>
        public async Task<byte[]> GetFileBytesAsync(string id)
        {
            return await _bucket.DownloadAsBytesAsync(new ObjectId(id));
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        public async Task<byte[]> GetFileBytesAsync(ObjectId id)
        {
            return await _bucket.DownloadAsBytesAsync(id);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Stream> GetFileStreamAsync(string id)
        {
            return await _bucket.OpenDownloadStreamAsync(new ObjectId(id));
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Stream> GetFileStreamAsync(ObjectId id)
        {
            return await _bucket.OpenDownloadStreamAsync(id);
        }

        /// <summary>
        /// 取得文件信息
        /// </summary>
        public async Task<GridFSFileInfo> GetFileByIdAsync(string id)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", new ObjectId(id));
            return await _bucket.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 取得文件信息
        /// </summary>
        public async Task<GridFSFileInfo> GetFileByIdAsync(ObjectId id)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", id);
            return await _bucket.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        public async Task<ObjectId> UploadFileAsync(string fileName, byte[] source)
        {
            var id = await _bucket.UploadFromBytesAsync(fileName, source);
            return id;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        public async Task<ObjectId> UploadFileAsync(string fileName, Stream source)
        {
            var id = await _bucket.UploadFromStreamAsync(fileName, source);
            return id;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        public async Task DeleteFileAsync(string id)
        {
            await _bucket.DeleteAsync(new ObjectId(id));
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        public async Task RenameFileAsync(string id, string newFilename)
        {
            await _bucket.RenameAsync(new ObjectId(id), newFilename);
        }

        #endregion

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
        /// 根据条件查询数据数量
        /// </summary>
        public long Count<T>(FilterDefinition<T> filter)
        {
            var total = GetCollection<T>().CountDocuments(filter);
            return total;
        }

        /// <summary>
        /// 根据条件查询数据数量
        /// </summary>
        public async Task<long> CountAsync<T>(FilterDefinition<T> filter)
        {
            var total = await GetCollection<T>().CountDocumentsAsync(filter);
            return total;
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
