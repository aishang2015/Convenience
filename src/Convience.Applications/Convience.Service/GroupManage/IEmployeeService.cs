using Convience.Model.Models.GroupManage;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Convience.Service.GroupManage
{
    public interface IEmployeeService
    {
        EmployeeResult GetEmployeeById(int id);

        (IEnumerable<EmployeeResult>, int) GetEmployees(EmployeeQuery query);

        Task<bool> UpdateEmplyeeAsync(EmployeeViewModel viewModel);
    }
}
