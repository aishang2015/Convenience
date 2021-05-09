using Microsoft.AspNetCore.Identity;

using System;

namespace Convience.Entity.Entity.Identity
{
    public class SystemUser : IdentityUser<int>
    {
        public string Avatar { get; set; }

        public Sex Sex { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedTime { get; set; }
    }

    public enum Sex
    {
        Unknown,
        Male,
        Famale
    }
}
