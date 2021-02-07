using Convience.EntityFrameWork.Repositories;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AppService.Service
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    /// <typeparam name="T">缓存数据库模型</typeparam>
    public interface ICachingService<T> where T : class
    {
        /// <summary>
        /// 清除缓存数据（同步）
        /// </summary>
        void ClearCacheData();

        /// <summary>
        /// 取得缓存数据（同步）
        /// </summary>
        /// <returns>数据</returns>
        IList<T> GetCacheData(int expire = 60);
    }

    public class CachingService<T> : ICachingService<T> where T : class
    {
        private readonly string _cacheKey = typeof(T).Name;

        private readonly IRepository<T> _repository;

        private readonly IMemoryCache _memoryCache;

        public CachingService(
            IRepository<T> repository,
            IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        #region 公开方法

        /// <summary>
        /// 清除缓存数据（同步）
        /// </summary>
        public void ClearCacheData()
        {
            _memoryCache.Remove(_cacheKey);
        }

        /// <summary>
        /// 取得缓存数据（同步）
        /// </summary>
        /// <returns>数据</returns>
        public IList<T> GetCacheData(int expire = 60)
        {
            var cacheList = _memoryCache.Get<List<T>>(_cacheKey);
            if (cacheList == null)
            {
                // 全量取出
                var dataList = _repository.Get().ToList();
                _memoryCache.Set(_cacheKey, dataList,
                    TimeSpan.FromSeconds(expire));
                return dataList;
            }
            return cacheList;
        }
        #endregion
    }
}
