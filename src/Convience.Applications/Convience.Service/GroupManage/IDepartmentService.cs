using Convience.Model.Models;
using Convience.Model.Models.GroupManage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.GroupManage
{
    public interface IDepartmentService
    {
        IQueryable<DepartmentResult> GetAllDepartment();

        Task<DepartmentResult> GetDepartmentById(int id);

        Task<bool> AddDepartmentAsync(DepartmentViewModel model);

        Task<bool> UpdateDepartmentAsync(DepartmentViewModel model);

        Task<bool> DeleteDepartmentAsync(int id);

        public IEnumerable<DicModel> GetDepartmentDic(string name);
    }
}
