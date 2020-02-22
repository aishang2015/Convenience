using System.Collections.Generic;

namespace backend.jwtauthentication
{
    public interface IJwtFactory
    {
        public string GenerateJwtToken(List<(string, string)> tuples = null);
    }
}
