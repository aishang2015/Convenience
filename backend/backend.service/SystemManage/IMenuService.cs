using Convience.Model.Models.SystemManage;

using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.SystemManage
{
    public interface IMenuService
    {
        IQueryable<MenuResult> GetAllMenu();

        Task<bool> AddMenuAsync(MenuViewModel model);

        Task<bool> UpdateMenuAsync(MenuViewModel model);

        Task<bool> DeleteMenuAsync(int id);

        bool HavePermission(string[] menuIds, string permission);

        (string, string) GetIdentificationRoutes(string[] menuIds);
    }
}
