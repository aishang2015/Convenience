using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Convience.EntityFrameWork.Saas
{
    public class DbContextCacheKey : ModelCacheKey
    {
        private readonly string _schema;

        public DbContextCacheKey(DbContext context) : base(context)
        {
            _schema = (context as AppSaasDbContext)?.Schema;
        }

        protected override bool Equals(ModelCacheKey other)
            => base.Equals(other)
           && (other as DbContextCacheKey)?._schema == _schema;

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode() * 397;
            if (_schema != null)
            {
                hashCode ^= _schema.GetHashCode();
            }

            return hashCode;
        }

    }
}
