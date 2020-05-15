using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

using System.Collections.Generic;

namespace Convience.Entity.Entity
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class DicType
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }

        public List<DicData> DicDatas { get; set; }
    }
}