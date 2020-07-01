
using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

using System;

namespace Convience.Entity.Entity
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class Tenant
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string UrlPrefix { get; set; }

        public DataBaseType DataBaseType { get; set; }

        public string ConnectionString { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
