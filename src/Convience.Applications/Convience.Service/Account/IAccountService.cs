using Convience.Entity.Data;

using System.Threading.Tasks;

namespace Convience.Service.Account
{
    public interface IAccountService
    {
        public Task<bool> IsStopUsing(string userName);

        public Task<(bool, string, SystemUser)> ValidateCredentials(string userName, string password);

        public Task<bool> ChangePassword(string userName, string oldPassword, string newPassword);
    }
}
