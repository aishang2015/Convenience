
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;

using System;
using System.IO;

namespace Convience.ManagentApi.Infrastructure.Logs.LoginLog
{
    public class LoginLogFilter : Attribute, IResultFilter
    {

        public void OnResultExecuted(ResultExecutedContext context)
        {
            var reader = new StreamReader(context.HttpContext.Request.Body);
            context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            var postBodyString = reader.ReadToEndAsync().Result;
            var body = JObject.Parse(postBodyString);

            var message = new LoginLogMessage();
            message.OperateAt = DateTime.Now;
            message.OperatorAccount = body["UserName"].ToString();
            message.IpAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            message.IsSuccess = context.HttpContext.Response.StatusCode == 200;
            LoginLogQueue.blockingCollection.Add(message);
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
        }
    }
}
