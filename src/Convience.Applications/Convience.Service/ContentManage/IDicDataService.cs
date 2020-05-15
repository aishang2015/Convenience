using Convience.Model.Models;
using Convience.Model.Models.ContentManage;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public interface IDicDataService
    {
        Task<DicDataResult> GetByIdAsync(int id);

        /// <summary>
        /// 根据字典类型编码取得对应字典数据
        /// </summary>
        /// <param name="dicTypeCode">字典类型编码</param>
        /// <returns>字典数据</returns>
        IEnumerable<DicModel> GetDicDataDic(string dicTypeCode);

        IEnumerable<DicDataResult> GetByDicTypeIdAsync(int dicTypeId);

        Task<bool> AddDicDataAsync(DicDataViewModel model);

        Task<bool> UpdateDicDataAsync(DicDataViewModel model);

        Task<bool> DeleteDicDataAsync(int id);
    }
}