using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Convience.EntityFrameWork.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(object key);
        IQueryable<TEntity> Get(bool tracking = false);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> where, bool tracking = false);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order,
            bool isDesc = false, bool tracking = false);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order,
            int page, int size, bool isDesc = false, bool tracking = false);
        IQueryable<TEntity> Get(IQueryable<TEntity> query, int page, int size);
        Task<long> CountAsync();
        Task<long> CountAsync(IQueryable<TEntity> query);

        Task<TEntity> AddAsync(TEntity entity);
        Task AddAsync(IEnumerable<TEntity> entities);

        Task RemoveAsync(object key);
        Task RemoveAsync(Expression<Func<TEntity, bool>> where);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void UpdatePartial(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
        void UpdateIgnore(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        Task<int> ExecuteSqlAsync(string sql);
    }
}
