using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.core.Authentication
{
    public class JwtOption
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpireSpan { get; set; }

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

        public DateTime Expires { get => DateTime.UtcNow.AddHours(ExpireSpan); }

        public SecurityKey SecurityKey { get; private set; }

        public SigningCredentials SigningCredentials { get; private set; }
    }
}
