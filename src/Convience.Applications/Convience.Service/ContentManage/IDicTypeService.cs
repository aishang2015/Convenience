using Convience.Model.Models.ContentManage;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public interface IDicTypeService
    {
        Task<DicTypeResult> GetByIdAsync(int id);

        IEnumerable<DicTypeResult> GetDicTypes();

        bool HasSameCode(string code);

        bool HasSameCode(int id, string code);

        Task<bool> AddDicTypeAsync(DicTypeViewModel model);

        Task<bool> UpdateDicTypeAsync(DicTypeViewModel model);

        Task<bool> DeleteDicTypeAsync(int id);
    }
}