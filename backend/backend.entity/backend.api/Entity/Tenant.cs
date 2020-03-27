using backend.data.Infrastructure;
using Backend.Entity.backend.api.Data;
using System;

namespace Backend.Entity.backend.api.Entity
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
