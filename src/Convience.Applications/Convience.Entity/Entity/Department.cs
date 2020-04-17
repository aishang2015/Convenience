using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Convience.Entity.Entity
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        [Description("部门负责人")]
        public int? LeaderId { get; set; }

        public int Sort { get; set; }
    }
}
