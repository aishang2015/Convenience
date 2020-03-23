using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Repository.backend.api.Data
{
    public class SystemUser : IdentityUser<int>
    {
        public string Avatar { get; set; }

        public Sex Sex { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public string RoleNames { get; set; }
    }

    public enum Sex
    {
        Unknown,
        Male,
        Famale
    }
}
