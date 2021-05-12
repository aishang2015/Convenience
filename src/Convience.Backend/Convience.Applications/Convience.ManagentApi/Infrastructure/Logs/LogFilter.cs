using Convience.JwtAuthentication;

using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.IO;

namespace Convience.ManagentApi.Infrastructure.Logs
{
    public class LogFilter : Attribute, IResultFilter
    {
        public string Module { get; private set; }

        public string SubModule { get; private set; }

        public string Function { get; private set; }

        public LogFilter(string module, string subodule, string function)
        {
            Module = module;
            SubModule = subodule;
            Function = function;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            var controllerType = context.Controller.GetType();
            var message = new OperateLogMessage();
            message.Controller = controllerType.FullName;
            message.Action = context.RouteData.Values["action"].ToString();
            message.HttpResultCode = context.HttpContext.Response.StatusCode.ToString();
            message.Uri = context.HttpContext.Request.Path;
            message.Account = context.HttpContext?.User?.GetUserName();
            message.Name = context.HttpContext?.User?.GetName();

            // todo 文件，form等特殊情况的处理
            var reader = new StreamReader(context.HttpContext.Request.Body);
            context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            message.RequestContent = reader.ReadToEndAsync().Result;

            OperateLogQueue.blockingCollection.Add(message);
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
        }
    }
}
