using Convience.Model.Models.DashBoard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Convience.Service.DashBoard
{
    public interface IDashBoardService
    {
        Task<DashBoardResult> GetAsync();
    }
}
