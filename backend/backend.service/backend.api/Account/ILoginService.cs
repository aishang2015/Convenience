using System.Threading.Tasks;

namespace backend.service.backend.api.Account
{
    public interface ILoginService
    {
        public Task<string> ValidateCredentials(string userName, string password);
    }
}
