using Convience.Model.Models.DashBoard;

using System.Threading.Tasks;

namespace Convience.Service.DashBoard
{
    public interface IDashBoardService
    {
        Task<DashBoardResult> GetAsync();
    }
}
