using System;

namespace Convience.WPFClient.Data.Entity
{
    public class LoginInfo
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public DateTime ExpireTime { get; set; }
    }
}
