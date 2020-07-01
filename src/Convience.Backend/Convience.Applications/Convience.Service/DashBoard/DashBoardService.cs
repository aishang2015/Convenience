using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.DashBoard;

using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.DashBoard
{
    public interface IDashBoardService
    {
        Task<DashBoardResultModel> GetAsync();
    }

    public class DashBoardService : IDashBoardService
    {
        private readonly IUserRepository _userRepository;

        private readonly IRoleRepository _roleRepository;

        private readonly IRepository<Department> _departmentRespository;

        private readonly IRepository<Position> _positionRespository;

        public DashBoardService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IRepository<Department> departmentRespository,
            IRepository<Position> positionRespository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _departmentRespository = departmentRespository;
            _positionRespository = positionRespository;
        }

        public async Task<DashBoardResultModel> GetAsync()
        {
            return new DashBoardResultModel()
            {
                UserCount = _userRepository.GetUsers().Count(),
                RoleCount = _roleRepository.GetRoles().Count(),
                DepartmentCount = await _departmentRespository.CountAsync(),
                PositionCount = await _positionRespository.CountAsync(),
            };
        }
    }
}
