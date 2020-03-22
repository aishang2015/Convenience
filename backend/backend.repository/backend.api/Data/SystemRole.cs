using Microsoft.AspNetCore.Identity;

using System.ComponentModel;

namespace Backend.Repository.backend.api.Data
{
    public class SystemRole : IdentityRole<int>
    {

        [Description("成员数量")]
        public int Number { get; set; }

        [Description("备注")]
        public string Remark { get; set; }
    }
}
