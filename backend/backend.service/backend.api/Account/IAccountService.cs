using System.Threading.Tasks;

namespace backend.service.backend.api.Account
{
    public interface IAccountService
    {
        public Task<string> ValidateCredentials(string userName, string password);

        public Task<bool> ChangePassword(string userName, string oldPassword, string newPassword);
    }
}
