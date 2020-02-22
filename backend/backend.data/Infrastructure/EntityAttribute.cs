﻿using System;

namespace backend.data.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : Attribute
    {
        public Type dbContextType { get; set; }
    }
}
