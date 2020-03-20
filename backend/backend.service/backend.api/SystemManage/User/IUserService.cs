using Backend.Model.backend.api.Models.SystemManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service.backend.api.SystemManage.User
{
    public interface IUserService
    {
        public IEnumerable<UserResult> GetUsers(UserQuery query);

        public Task<UserResult> GetUserAsync(string Id);

        public Task<bool> AddUserAsync(UserViewModel model);

        public Task UpdateUserAsync(UserViewModel model);

        public Task<bool> RemoveUserAsync(string Id);
    }
}
