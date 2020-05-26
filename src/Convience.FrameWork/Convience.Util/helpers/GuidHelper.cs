using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace Convience.Util.Helpers
{
    public static class GuidHelper
    {
        // ef core SQL Server有序guid生成器
        private static ValueGenerator<Guid> _generator = new SequentialGuidValueGenerator();

        public static Guid NewSquentialGuid()
        {
            return _generator.Next(null);
        }
    }
}
