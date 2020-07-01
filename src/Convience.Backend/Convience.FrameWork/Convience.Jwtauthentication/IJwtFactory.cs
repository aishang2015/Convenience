using System.Collections.Generic;

namespace Convience.JwtAuthentication
{
    public interface IJwtFactory
    {
        public string GenerateJwtToken(List<(string, string)> tuples = null);
    }
}
