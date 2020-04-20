using Convience.Model.Models.GroupManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Convience.Service.GroupManage
{
    public class EmployeeService : IEmployeeService
    {
        public IEnumerable<EmployeeResult> GetEmployees(EmployeeQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEmplyee(EmployeeViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
