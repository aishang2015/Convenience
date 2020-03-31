using System;

namespace Convience.EntityFrameWork.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : Attribute
    {
        public Type DbContextType { get; set; }
    }
}
