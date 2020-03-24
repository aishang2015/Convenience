using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Infrastructure.Authorization
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(string name) : base(typeof(PermissionFilter))
        {
            Arguments = new[] { name };
        }
    }
}
