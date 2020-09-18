using Convience.EntityFrameWork.Repositories;

namespace Convience.Entity.Data
{
    public class SystemIdentityDbUnitOfWork : UnitOfWork<SystemIdentityDbContext>
    {
        public SystemIdentityDbUnitOfWork(SystemIdentityDbContext systemIdentityDbContext)
            : base(systemIdentityDbContext) { }
    }

}
