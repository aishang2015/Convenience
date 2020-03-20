using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model.backend.api.Models.SystemManage
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public int Sex { get; set; }

        public string Avatar { get; set; }

        public string Name { get; set; }

        public string RoleNames { get; set; }

        public bool IsActive { get; set; }

        public string Password { get; set; }
    }
}
