using AutoMapper;
using backend.entity.backend.api;
using backend.repository.backend.api;
using Backend.Model.backend.api.Models.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service.backend.api.SystemManage.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddUserAsync(UserViewModel model)
        {
            var user = _mapper.Map<SystemUser>(model);
            return await _userRepository.AddUserAsync(user);
        }

        public async Task<UserResult> GetUserAsync(string Id)
        {
            var user = await _userRepository.GetUserByIdAsync(Id);
            return _mapper.Map<UserResult>(user);
        }

        public IEnumerable<UserResult> GetUsers(UserQuery query)
        {
            Expression<Func<SystemUser, bool>> where = u => (
                (string.IsNullOrEmpty(query.UserName) || u.UserName.Contains(query.UserName)) &&
                (string.IsNullOrEmpty(query.Name) || u.Name.Contains(query.Name)) &&
                (string.IsNullOrEmpty(query.PhoneNumber) || u.PhoneNumber.Contains(query.PhoneNumber)) &&
                (string.IsNullOrEmpty(query.RoleName) || u.Name.Contains(query.RoleName))
            );
            var users = _userRepository.GetUsers(where, query.Page, query.Size).ToArray();
            return _mapper.Map<SystemUser[], IEnumerable<UserResult>>(users);
        }

        public async Task<bool> RemoveUserAsync(string Id)
        {
            return await _userRepository.RemoveUserByIdAsync(Id);
        }

        public async Task UpdateUserAsync(UserViewModel model)
        {
            var user = _mapper.Map<SystemUser>(model);
            await _userRepository.UpdateUserAsync(user);
        }
    }
}
