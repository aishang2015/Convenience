using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

using System;

namespace Convience.Entity.Entity.Tenants
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class Tenant
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Schema { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
