using Microsoft.AspNetCore.Http;

using System.Threading.Tasks;

namespace Convience.Util.Middlewares
{
    public class PathMiddleware
    {
        private readonly RequestDelegate _next;


        //在应用程序的生命周期中，中间件的构造函数只会被调用一次
        public PathMiddleware(RequestDelegate next
            )
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 解析请求路径
            var path = context.Request.Path.ToUriComponent().ToLowerInvariant();
            var isSuccess = true;

            // todo
            // 执行一些根据路径去处理的逻辑

            if (isSuccess)
            {
                await _next(context);
            }
            else
            {
                await context.Response.WriteAsync("Your request is abandoned!");
            }
        }
    }
}
