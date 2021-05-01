using System;
using System.Collections.Generic;

namespace Convience.JwtAuthentication
{
    public interface IJwtFactory
    {
        /// <summary>
        /// 生成jwt，token
        /// </summary>
        public string GenerateJwtToken(List<(string, string)> tuples = null);

        /// <summary>
        /// 获取token过期时间
        /// </summary>
        public DateTime GetJwtExpireTime();
    }
}
