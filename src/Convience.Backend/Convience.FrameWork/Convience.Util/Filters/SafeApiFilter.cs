using Convience.Util.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;

namespace Convience.Util.Filters
{
    public class SafeApiFilter : IAsyncActionFilter
    {
        private readonly ILogger<SafeApiFilter> _logger;

        public SafeApiFilter(ILogger<SafeApiFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var headers = context.HttpContext.Request.Headers;
            var timestamp = headers["TimeStamp"];
            var nonce = headers["Nonce"];
            var sign = headers["Sign"];

            var md5 = EncryptionHelper.MD5Encrypt(timestamp.FirstOrDefault() + "-" + nonce.FirstOrDefault());

            if (md5 == sign.FirstOrDefault())
            {
                _logger.LogDebug("签名验证成功！");
                await next();
            }
            else
            {
                _logger.LogDebug("签名验证失败！");
                context.Result = new StatusCodeResult(406);
            }
        }
    }
}
