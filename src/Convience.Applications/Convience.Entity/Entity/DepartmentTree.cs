using System;
using System.Collections.Generic;
using System.Text;

namespace Convience.Entity.Entity
{
    public class DepartmentTree
    {
        public int Id { get; set; }

        public int Ancestor { get; set; }

        public int Descendant { get; set; }

        public int Length { get; set; }
    }
}
