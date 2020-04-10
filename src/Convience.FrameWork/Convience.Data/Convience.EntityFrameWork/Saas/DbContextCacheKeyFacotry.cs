using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

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
