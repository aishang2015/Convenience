using backend.data.Infrastructure;

namespace backend.entity.Entities
{
    [Entity(DbContextType = typeof(AppIdentityDbContext))]
    public class TestEntity
    {
        public string Id { get; set; }

        public string Desc { get; set; }
    }
}
