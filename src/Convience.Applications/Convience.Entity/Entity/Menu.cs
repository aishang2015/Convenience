
using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

using System.ComponentModel;

namespace Convience.Entity.Entity
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

        public Menu() { }

        public Menu(int id, string name, string identification,
            string permission, int type, string route, int sort)
        {
            Id = id;
            Name = name;
            Identification = identification;
            Permission = permission;
            Type = (MenuType)type;
            Route = route;
            Sort = sort;
        }
    }

    public enum MenuType
    {
        未知 = 0,
        菜单 = 1,
        按钮 = 2,
        链接 = 3,
    }

}
