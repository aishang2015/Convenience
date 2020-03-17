using Microsoft.AspNetCore.Identity;

namespace backend.entity.backend.api
{
    public class SystemUser : IdentityUser
    {
        public string Avatar { get; set; }

        public string Name { get; set; }

        public int Sex { get; set; }

        public int DepartmentId { get; set; }
    }
}
