using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace backend.data.Repositories
{
    public interface IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        Task CommitAsync();
    }
}
