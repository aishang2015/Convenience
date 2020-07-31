using Convience.Caching;
using Convience.EntityFrameWork.Repositories;
using EasyCaching.Core;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppService.Service
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    /// <typeparam name="T">缓存数据库模型</typeparam>
    public interface ICachingService<T> where T : class
    {
        /// <summary>
        /// 取得缓存数据(同步)
        /// </summary>
        /// <returns>数据</returns>
        IList<T> GetCacheData();

        /// <summary>
        /// 清除缓存数据（同步）
        /// </summary>
        void ClearCacheData();

        /// <summary>
        /// 取得缓存数据（异步）
        /// </summary>
        /// <returns>数据</returns>
        Task<IList<T>> GetCacheDataAsync();

        /// <summary>
        /// 清除缓存数据（异步）
        /// </summary>
        Task ClearCacheDataAsync();
    }

    public class CachingService<T> : ICachingService<T> where T : class
    {
        private readonly string _cacheKey = typeof(T).Name;

        private readonly IRepository<T> _repository;

        private readonly IEasyCachingProvider _provider;

        private readonly CachingOption _cachingOption;

        public CachingService(
            IRepository<T> repository,
            IEasyCachingProvider provider,
            IOptions<CachingOption> options)
        {
            _repository = repository;
            _provider = provider;
            _cachingOption = options.Value;
        }

        #region 公开方法

        /// <summary>
        /// 清除缓存数据（异步）
        /// </summary>
        public async Task ClearCacheDataAsync()
        {
            await _provider.RemoveAsync(_cacheKey);
        }

        /// <summary>
        /// 取得缓存数据（异步）
        /// </summary>
        /// <returns>数据</returns>
        public async Task<IList<T>> GetCacheDataAsync()
        {
            var cacheList = await _provider.GetAsync<List<T>>(_cacheKey);
            if (!cacheList.HasValue)
            {
                // 全量取出
                var dataList = _repository.Get().ToList();
                await _provider.SetAsync(_cacheKey, dataList,
                    TimeSpan.FromSeconds(_cachingOption.ExpireSpan));
                return dataList;
            }
            return cacheList.Value;
        }

        /// <summary>
        /// 清除缓存数据（同步）
        /// </summary>
        public void ClearCacheData()
        {
            _provider.Remove(_cacheKey);
        }

        /// <summary>
        /// 取得缓存数据(同步)
        /// </summary>
        /// <returns>数据</returns>
        public IList<T> GetCacheData()
        {
            var cacheList = _provider.Get<List<T>>(_cacheKey);
            if (!cacheList.HasValue)
            {
                // 全量取出
                var dataList = _repository.Get().ToList();
                _provider.Set(_cacheKey, dataList, TimeSpan.FromSeconds(_cachingOption.ExpireSpan));
                return dataList;
            }
            return cacheList.Value;
        }
        #endregion
    }
}
