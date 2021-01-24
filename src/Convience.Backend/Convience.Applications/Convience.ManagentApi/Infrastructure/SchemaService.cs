using Convience.EntityFrameWork.Saas;
using Convience.JwtAuthentication;

using Microsoft.AspNetCore.Http;

namespace Convience.ManagentApi.Infrastructure
{
    public class SchemaService : ISchemaService
    {
        public string Schema { get; set; }

        public SchemaService(IHttpContextAccessor httpContextAccessor)
        {
            Schema = httpContextAccessor.HttpContext?.User?.GetUserSchema();
        }
    }
}
