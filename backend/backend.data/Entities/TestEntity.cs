using backend.core.Data;
using backend.data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace backend.data.Entities
{
    [Entity(dbContextType = typeof(ApplicationDbContext))]
    public class TestEntity
    {
        public string Id { get; set; }

        public string Desc { get; set; }
    }
}
