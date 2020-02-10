using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace backend.data.Repositories
{
    public class BaseRepository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;

        private readonly DbSet<TEntity> _dataSet;

        public BaseRepository(TDbContext dbContext)
        {
            _context = dbContext;
            _dataSet = dbContext.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _dataSet.AddAsync(entity);
            return result.Entity;
        }

        public async Task AddAsync(IEnumerable<TEntity> entities)
        {
            await _dataSet.AddRangeAsync(entities);
        }

        public async Task<TEntity> GetAsync(object key)
        {
            return await _dataSet.FindAsync(key);
        }

        public IQueryable<TEntity> Get(bool tracking = false)
        {
            return tracking ? _dataSet.AsTracking() : _dataSet.AsNoTracking();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> where, bool tracking = false)
        {
            return tracking ? _dataSet.Where(where).AsTracking() : _dataSet.Where(where).AsNoTracking();
        }

        public IQueryable<TEntity> Get(IQueryable<TEntity> query,
            Expression<Func<TEntity, object>> order, bool isDesc = false, bool tracking = false)
        {
            query = isDesc ? query.OrderByDescending(order) : query.OrderBy(order);
            query = tracking ? query.AsTracking() : query.AsNoTracking();
            return query;
        }

        public IQueryable<TEntity> Get(IQueryable<TEntity> query, int page, int size, bool tracking = false)
        {
            var skip = size * (page - 1);
            return query.Skip(skip).Take(size);
        }

        public async Task RemoveAsync(object key)
        {
            var entity = await GetAsync(key);
            _dataSet.Remove(entity);
        }

        public async Task RemoveAsync(Expression<Func<TEntity, bool>> where)
        {
            var entities = await Get(where).ToArrayAsync();
            _dataSet.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _dataSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dataSet.UpdateRange(entities);
        }

        public async Task<int> CountAsync(IQueryable<TEntity> query)
        {
            return await query.CountAsync();
        }
    }
}
