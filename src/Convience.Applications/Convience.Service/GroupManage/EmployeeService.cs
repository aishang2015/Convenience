using Convience.Entity.Data;
using Convience.EntityFrameWork.Repositories;
using Convience.Jwtauthentication;
using Convience.Model.Models.GroupManage;
using Convience.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.GroupManage
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUserRepository _userRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly SystemIdentityDbContext _systemIdentityDbContext;

        public EmployeeService(IUserRepository userRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            SystemIdentityDbContext systemIdentityDbContext)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _systemIdentityDbContext = systemIdentityDbContext;
        }

        public IEnumerable<EmployeeResult> GetEmployees(EmployeeQuery query)
        {
            var users = from u in _systemIdentityDbContext.Users
                        let pquery = from uc in _systemIdentityDbContext.UserClaims
                                     where u.Id == uc.Id && uc.ClaimType == CustomClaimTypes.UserPosition
                                     select uc.ClaimValue
                        let dquery = from uc in _systemIdentityDbContext.UserClaims
                                     where u.Id == uc.Id && uc.ClaimType == CustomClaimTypes.UserDepartment
                                     select uc.ClaimValue
                        select new EmployeeResult
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            PhoneNumber = u.PhoneNumber,
                            Avatar = u.Avatar,
                            Name = u.Name,
                            Sex = (int)u.Sex,
                            DepartmentId = dquery.FirstOrDefault(),
                            PositionIds = string.Join(',', pquery)
                        };
            return users;
        }

        public Task<bool> UpdateEmplyeeAsync(EmployeeViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
