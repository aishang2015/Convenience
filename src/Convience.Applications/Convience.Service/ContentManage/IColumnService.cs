using Convience.Model.Models.ContentManage;

using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public interface IColumnService
    {
        Task<ColumnResult> GetByIdAsync(int id);

        IQueryable<ColumnResult> GetAllColumn();

        Task<bool> AddColumnAsync(ColumnViewModel model);

        Task<bool> UpdateColumnAsync(ColumnViewModel model);

        Task<bool> DeleteColumnAsync(int id);
    }
}
