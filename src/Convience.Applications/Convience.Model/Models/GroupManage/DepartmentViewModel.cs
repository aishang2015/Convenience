using System;
using System.Collections.Generic;
using System.Text;

namespace Convience.Model.Models.GroupManage
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        public string UpId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public int? LeaderId { get; set; }

        public int Sort { get; set; }
    }
}
