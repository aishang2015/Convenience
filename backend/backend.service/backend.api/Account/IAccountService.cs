using Backend.Entity.backend.api.Data;
using System.Threading.Tasks;

namespace backend.service.backend.api.Account
{
    public interface IAccountService
    {
        public Task<bool> IsStopUsing(string userName);

        public Task<(bool, string, SystemUser)> ValidateCredentials(string userName, string password);

        public Task<bool> ChangePassword(string userName, string oldPassword, string newPassword);
    }
}
