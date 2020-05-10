using Convience.Model.Models.ContentManage;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public interface IDicDataService
    {
        Task<DicDataResult> GetByIdAsync(int id);

        IEnumerable<DicDataResult> GetByDicTypeIdAsync(int dicTypeId);

        Task<bool> AddDicDataAsync(DicDataViewModel model);

        Task<bool> UpdateDicDataAsync(DicDataViewModel model);

        Task<bool> DeleteDicDataAsync(int id);
    }
}