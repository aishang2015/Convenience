using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Convience.EntityFrameWork.Saas
{
    public class DbContextCacheKeyFacotry : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
        {
            return new DbContextCacheKey(context);
        }
    }
}
