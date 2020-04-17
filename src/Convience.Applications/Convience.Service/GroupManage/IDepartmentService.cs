using Convience.Model.Models.GroupManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convience.Service.GroupManage
{
    public interface IDepartmentService
    {
        IQueryable<DepartmentResult> GetAllDepartment();

        Task<bool> AddDepartmentAsync(DepartmentViewModel model);

        Task<bool> UpdateDepartmentAsync(DepartmentViewModel model);

        Task<bool> DeleteDepartmentAsync(int id);
    }
}
