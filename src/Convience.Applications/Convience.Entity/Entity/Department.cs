using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

using System.ComponentModel;

namespace Convience.Entity.Entity
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        [Description("部门负责人")]
        public int? LeaderId { get; set; }

        public int Sort { get; set; }
    }
}
