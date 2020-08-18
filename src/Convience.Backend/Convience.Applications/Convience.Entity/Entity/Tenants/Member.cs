using Convience.EntityFrameWork.Infrastructure;
using Convience.EntityFrameWork.Saas;

namespace Convience.Entity.Entity.Tenants
{
    [Entity(DbContextType = typeof(AppSaasDbContext))]
    public class Member
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Salt { get; set; }

        public string PasswordHash { get; set; }
    }
}
