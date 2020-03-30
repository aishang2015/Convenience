using System.Collections.Generic;

namespace Convience.Jwtauthentication
{
    public interface IJwtFactory
    {
        public string GenerateJwtToken(List<(string, string)> tuples = null);
    }
}
