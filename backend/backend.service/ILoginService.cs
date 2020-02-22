using System.Threading.Tasks;

namespace backend.service
{
    public interface ILoginService
    {
        public Task<string> ValidateCredentials(string userName, string password);
    }
}
