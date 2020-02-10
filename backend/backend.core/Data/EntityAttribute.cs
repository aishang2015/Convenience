using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace backend.core.Data
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : Attribute
    {
        public Type dbContextType { get; set; }
    }
}
