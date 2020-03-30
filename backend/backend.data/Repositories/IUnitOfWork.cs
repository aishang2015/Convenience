using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace Convience.EntityFrameWork.Repositories
{
    public interface IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        Task SaveAsync();

        Task StartTransactionAsync();

        Task RollBackAsync();

        Task CommitAsync();
    }
}
