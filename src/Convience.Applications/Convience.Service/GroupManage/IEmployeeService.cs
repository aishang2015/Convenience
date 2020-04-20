using Convience.Model.Models.GroupManage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Convience.Service.GroupManage
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeResult> GetEmployees(EmployeeQuery query);

        Task<bool> UpdateEmplyeeAsync(EmployeeViewModel viewModel);
    }
}
