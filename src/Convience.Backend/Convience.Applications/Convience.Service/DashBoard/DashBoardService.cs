using Convience.Entity.Entity;
using Convience.Entity.Entity.Identity;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.DashBoard;
using System.Threading.Tasks;

namespace Convience.Service.DashBoard
{
    public interface IDashBoardService
    {
        Task<DashBoardResultModel> GetAsync();
    }

    public class DashBoardService : IDashBoardService
    {
        private readonly IRepository<SystemUser> _userRepository;

        private readonly IRepository<SystemRole> _roleRepository;

        private readonly IRepository<Department> _departmentRespository;

        private readonly IRepository<Position> _positionRespository;

        public DashBoardService(
            IRepository<SystemUser> userRepository,
            IRepository<SystemRole> roleRepository,
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
                UserCount = await _userRepository.CountAsync(),
                RoleCount = await _roleRepository.CountAsync(),
                DepartmentCount = await _departmentRespository.CountAsync(),
                PositionCount = await _positionRespository.CountAsync(),
            };
        }
    }
}
