using Microsoft.AspNetCore.Identity;

using System.ComponentModel;

namespace Backend.Repository.backend.api.Data
{
    public class SystemRole : IdentityRole<int>
    {
        [Description("备注")]
        public string Remark { get; set; }
    }
}
