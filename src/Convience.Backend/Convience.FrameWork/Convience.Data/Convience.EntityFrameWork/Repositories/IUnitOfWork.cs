using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using System.Threading.Tasks;

namespace Convience.EntityFrameWork.Repositories
{
    public interface IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 自动事务
        /// </summary>
        Task SaveAsync();

        /// <summary>
        /// 并发保存（程序端优先）
        /// </summary>
        Task SaveByClientAsync(int retryCount = 3);

        /// <summary>
        /// 并发保存（合并程序端和数据库）
        /// </summary>
        Task SaveByMergeAsync(int retryCount = 3);

        /// <summary>
        /// 开启手动事务
        /// </summary>
        Task<IDbContextTransaction> StartTransactionAsync();

        /// <summary>
        /// 手动事务回滚
        /// </summary>
        Task RollBackAsync(IDbContextTransaction transaction);

        /// <summary>
        /// 手动事务提交
        /// </summary>
        Task CommitAsync(IDbContextTransaction transaction);
    }
}
