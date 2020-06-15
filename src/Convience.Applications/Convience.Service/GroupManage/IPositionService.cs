using Convience.Model.Models;
using Convience.Model.Models.GroupManage;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Convience.Service.GroupManage
{
    public interface IPositionService
    {
        int Count();

        IEnumerable<PositionResult> GetAllPosition();

        IEnumerable<PositionResult> GetPositions(PositionQuery query);

        Task<PositionResult> GetPositionAsync(int id);

        Task<bool> AddPositionAsync(PositionViewModel model);

        Task<bool> UpdatePositionAsync(PositionViewModel model);

        Task<bool> DeletePositionAsync(int id);

        public IEnumerable<DicModel> GetPositionDic(string name);
    }
}
