using Backend.Model.backend.api.Models.SystemManage;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Service.backend.api.SystemManage.User
{
    public interface IUserService
    {
        public int Count();

        public IEnumerable<UserResult> GetUsers(UserQuery query);

        public Task<UserResult> GetUserAsync(string Id);

        public Task<string> AddUserAsync(UserViewModel model);

        public Task<string> UpdateUserAsync(UserViewModel model);

        public Task<string> RemoveUserAsync(string Id);

        public Task<string> RemoveUserByNameAsync(string Name);

        public Task<bool> SetUserPassword(string userName, string password);

        public Task<bool> ResetUserPassword(string userName, string password);

        public Task<IEnumerable<string>> GetUserRoles(string userName);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
    }
}
