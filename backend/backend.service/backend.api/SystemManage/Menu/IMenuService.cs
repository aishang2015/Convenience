using Backend.Model.backend.api.Models.SystemManage;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Service.backend.api.SystemManage.Menu
{
    public interface IMenuService
    {
        IQueryable<MenuResult> GetAllMenu();

        Task<bool> AddMenuAsync(MenuViewModel model);

        Task<bool> UpdateMenuAsync(MenuViewModel model);

        Task<bool> DeleteMenuAsync(int id);
    }
}
