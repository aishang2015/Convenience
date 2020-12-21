using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Convience.EntityFrameWork.Repositories
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

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> order, bool isDesc = false, bool tracking = false)
        {
            var query = tracking ? _dataSet.Where(where).AsTracking() : _dataSet.Where(where).AsNoTracking();
            query = isDesc ? query.OrderByDescending(order) : query.OrderBy(order);
            query = tracking ? query.AsTracking() : query.AsNoTracking();
            return query;
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order,
            int page, int size, bool isDesc = false, bool tracking = false)
        {
            var query = tracking ? _dataSet.Where(where).AsTracking() : _dataSet.Where(where).AsNoTracking();
            query = isDesc ? query.OrderByDescending(order) : query.OrderBy(order);
            query = tracking ? query.AsTracking() : query.AsNoTracking();
            var skip = size * (page - 1);
            return query.Skip(skip).Take(size);
        }

        public IQueryable<TEntity> Get(IQueryable<TEntity> query, int page, int size)
        {
            var skip = size * (page - 1);
            return query.Skip(skip).Take(size);
        }

        public IQueryable<TEntity> Get(string sql)
        {
            return _dataSet.FromSqlRaw(sql);
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

        public async Task<long> CountAsync(IQueryable<TEntity> query)
        {
            return await query.CountAsync();
        }

        public async Task<long> CountAsync()
        {
            return await _dataSet.CountAsync();
        }

        public Task<int> ExecuteSqlAsync(string sql)
        {
            return _context.Database.ExecuteSqlRawAsync(sql);
        }

        public void UpdatePartial(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            _context.Attach(entity);
            foreach (var expression in properties)
            {
                _context.Entry(entity).Property(expression).IsModified = true;
            }
        }

        public void UpdateIgnore(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            _context.Entry(entity).State = EntityState.Modified;
            foreach (var expression in properties)
            {
                _context.Entry(entity).Property(expression).IsModified = false;
            }
        }

        public void DetachAll()
        {
            foreach (var entity in _context.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached;
            }
        }
    }
}
