
using Microsoft.AspNetCore.Authentication.JwtBearer;

using System.Threading.Tasks;

namespace Convience.SignalRs
{
    public class SignalRJwtBearerEvents : JwtBearerEvents
    {
        private readonly string _path;

        public SignalRJwtBearerEvents(string path = "/hubs")
        {
            _path = path;
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments(_path))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return base.MessageReceived(context);
        }
    }
}
