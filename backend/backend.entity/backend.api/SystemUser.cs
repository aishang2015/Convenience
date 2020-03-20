using Microsoft.AspNetCore.Identity;

namespace backend.entity.backend.api
{
    public class SystemUser : IdentityUser<int>
    {
        public string Avatar { get; set; }

        public Sex Sex { get; set; }

        public string Name { get; set; }

        public string RoleNames { get; set; }

        public bool IsActive { get; set; }
    }

    public enum Sex
    {
        Unknown,
        Male,
        Famale
    }
}
