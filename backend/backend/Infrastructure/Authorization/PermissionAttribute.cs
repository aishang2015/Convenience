using Microsoft.AspNetCore.Mvc;

namespace Convience.ManagentApi.Infrastructure.Authorization
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(string name) : base(typeof(PermissionFilter))
        {
            Arguments = new[] { name };
        }
    }
}
