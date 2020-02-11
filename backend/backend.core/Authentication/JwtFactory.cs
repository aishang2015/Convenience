using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.core.Authentication
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtOption _jwtOption;

        public JwtFactory(IOptions<JwtOption> jwtOption)
        {
            _jwtOption = jwtOption.Value;
        }

        public string GenerateJwtToken(List<(string, string)> tuples = null)
        {
            var userClaims = new List<Claim>();
            if (tuples != null)
            {
                foreach (var tuple in tuples)
                {
                    userClaims.Add(new Claim(tuple.Item1, tuple.Item2));
                }
            }
            var jwtSecurityToken = new JwtSecurityToken(
                expires: _jwtOption.Expires,
                claims: userClaims,
                signingCredentials: _jwtOption.SigningCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }
    }
}
