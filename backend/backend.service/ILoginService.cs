using backend.data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace backend.service
{
    public interface ILoginService
    {
        public Task<string> ValidateCredentials(string userName, string password);
    }
}
