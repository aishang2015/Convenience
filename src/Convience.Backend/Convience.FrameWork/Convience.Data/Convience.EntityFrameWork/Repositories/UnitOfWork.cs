using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.EntityFrameWork.Repositories
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public UnitOfWork(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 客户端值优先，覆盖服务器值
        /// </summary>
        public async Task SaveByClientAsync(int retryCount = 3)
        {
            var isOk = false;
            while (!isOk && retryCount-- > 0)
            {
                try
                {
                    await _dbContext.SaveChangesAsync();
                    isOk = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entity in ex.Entries)
                    {
                        await entity.ReloadAsync();
                    }
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 变更数据以数据库优先，未变更以客户端优先
        /// </summary>
        public async Task SaveByMergeAsync(int retryCount = 3)
        {
            var isOk = false;
            while (!isOk && retryCount-- > 0)
            {
                try
                {
                    await _dbContext.SaveChangesAsync();
                    isOk = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entity in ex.Entries)
                    {
                        // 当前值
                        var proposedValues = entity.CurrentValues;

                        // 数据库最新值
                        var databaseValues = entity.GetDatabaseValues();

                        // 原始值
                        var originalValues = entity.OriginalValues;

                        entity.OriginalValues.SetValues(databaseValues);

                        databaseValues.Properties
                            .Where(property => !Equals(originalValues[property], databaseValues[property]))
                            .ToList().ForEach(property => entity.Property(property.Name).IsModified = false);
                    }
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<IDbContextTransaction> StartTransactionAsync()
        {
            return await _dbContext?.Database.BeginTransactionAsync();
        }

        public async Task RollBackAsync(IDbContextTransaction transaction)
        {
            await transaction?.RollbackAsync();
        }

        public async Task CommitAsync(IDbContextTransaction transaction)
        {
            await transaction?.CommitAsync();
        }
    }
}
