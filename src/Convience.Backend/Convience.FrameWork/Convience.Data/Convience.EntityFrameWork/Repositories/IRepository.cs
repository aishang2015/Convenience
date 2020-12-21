using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Convience.EntityFrameWork.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Retrieve
        Task<TEntity> GetAsync(object key);
        IQueryable<TEntity> Get(bool tracking = false);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> where, bool tracking = false);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order,
            bool isDesc = false, bool tracking = false);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order,
            int page, int size, bool isDesc = false, bool tracking = false);
        IQueryable<TEntity> Get(IQueryable<TEntity> query, int page, int size);
        IQueryable<TEntity> Get(string sql);
        Task<long> CountAsync();
        Task<long> CountAsync(IQueryable<TEntity> query);
        #endregion

        #region Create
        Task<TEntity> AddAsync(TEntity entity);
        Task AddAsync(IEnumerable<TEntity> entities);
        #endregion

        #region Delete
        Task RemoveAsync(object key);
        Task RemoveAsync(Expression<Func<TEntity, bool>> where);
        #endregion

        #region Update
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新指定属性，其他属性不更新
        /// </summary>
        void UpdatePartial(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        /// <summary>
        /// 不更新指定属性，其他属性更新
        /// </summary>
        void UpdateIgnore(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        /// <summary>
        /// 取消跟踪所有实体
        /// </summary>
        void DetachAll();
        #endregion

        Task<int> ExecuteSqlAsync(string sql);
    }
}
