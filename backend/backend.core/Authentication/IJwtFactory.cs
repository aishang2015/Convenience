using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.core.Authentication
{
    public interface IJwtFactory
    {
        public string GenerateJwtToken(List<(string, string)> tuples = null);
    }
}
