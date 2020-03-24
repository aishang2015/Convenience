using backend.data.Infrastructure;
using Backend.Entity.backend.api.Data;
using System.ComponentModel;

namespace Backend.Entity.backend.api.Entity
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class Menu
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Description("前端识别")]
        public string Identification { get; set; }

        [Description("后端权限")]
        public string Permission { get; set; }

        public MenuType Type { get; set; }

        public string Route { get; set; }

        public int Sort { get; set; }
    }

    public enum MenuType
    {
        未知 = 0,
        菜单 = 1,
        按钮 = 2,
        链接 = 3,
    }

}
