using Microsoft.IdentityModel.Tokens;

using System;
using System.Text;

namespace Convience.JwtAuthentication
{
    public class JwtOption
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public double ExpireSpan { get; set; }

        private string _secretKey;

        public string SecretKey
        {
            set
            {
                _secretKey = value;
                SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(value));
                SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            }
            get => _secretKey;
        }

        public DateTime Expires { get => DateTime.Now.AddHours(ExpireSpan); }

        public SecurityKey SecurityKey { get; private set; }

        public SigningCredentials SigningCredentials { get; private set; }
    }
}
