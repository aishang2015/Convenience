using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model.backend.api.Models.SystemManage
{
    public class UserQuery
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public string RoleName { get; set; }
    }
}
