using Convience.Entity.Entity;
using Convience.Model.Models.GroupManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convience.Service.GroupManage
{
    public interface IPositionService
    {
        IEnumerable<PositionResult> GetAllPosition();

        IEnumerable<PositionResult> GetPositions(PositionQuery query);

        Task<bool> AddPositionAsync(PositionViewModel model);

        Task<bool> UpdatePositionAsync(PositionViewModel model);

        Task<bool> DeletePositionAsync(int id);
    }
}
