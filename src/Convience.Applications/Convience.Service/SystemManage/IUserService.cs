using Convience.Model.Models;
using Convience.Model.Models.SystemManage;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Convience.Service.SystemManage
{
    public interface IUserService
    {
        public int Count();

        public IEnumerable<DicModel> GetUserDic(string name);

        public IEnumerable<UserResult> GetUsers(UserQuery query);

        public Task<UserResult> GetUserAsync(string Id);

        public Task<string> AddUserAsync(UserViewModel model);

        public Task<string> UpdateUserAsync(UserViewModel model);

        public Task<string> RemoveUserAsync(string Id);

        public Task<string> RemoveUserByNameAsync(string Name);

        public Task<bool> SetUserPassword(string userName, string password);

        public Task<bool> ResetUserPassword(string userName, string password);
    }
}
