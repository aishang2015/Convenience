using System;
using System.Threading;

namespace Convience.Util.Helpers
{
    public static class GuidHelper
    {
        private static SequentialGuidValueGenerator _generator = new SequentialGuidValueGenerator();

        #region 基于时间戳

        public static Guid NewSquentialGuid()
        {
            return _generator.Next();
        }

        public static string NewSquentialGuidString()
        {
            // 1f3c6041-c68f-4ab3-ae19-f66f541e3209
            return _generator.Next().ToString();
        }

        public static string NewSquentialGuidNoDash()
        {
            // 1f3c6041c68f4ab3ae19f66f541e3209
            return NewSquentialGuid().ToString("N");
        }

        public static string NewSquentialGuidWithBrace()
        {
            // {1f3c6041-c68f-4ab3-ae19-f66f541e3209}
            return NewSquentialGuid().ToString("B");
        }

        public static string NewSquentialGuidWithParentheses()
        {
            // (1f3c6041-c68f-4ab3-ae19-f66f541e3209)
            return NewSquentialGuid().ToString("P");
        }

        #endregion

        #region 普通版
        public static Guid NewGuid()
        {
            return Guid.NewGuid();
        }

        public static string NewGuidString()
        {
            // 1f3c6041-c68f-4ab3-ae19-f66f541e3209
            return Guid.NewGuid().ToString();
        }

        public static string NewGuidNoDash()
        {
            // 1f3c6041c68f4ab3ae19f66f541e3209
            return NewGuid().ToString("N");
        }

        public static string NewGuidWithBrace()
        {
            // {1f3c6041-c68f-4ab3-ae19-f66f541e3209}
            return NewGuid().ToString("B");
        }

        public static string NewGuidWithParentheses()
        {
            // (1f3c6041-c68f-4ab3-ae19-f66f541e3209)
            return NewGuid().ToString("P");
        }

        #endregion 
    }

    // from efcore/src/EFCore/ValueGeneration/SequentialGuidValueGenerator.cs
    public class SequentialGuidValueGenerator
    {
        private long _counter = DateTime.UtcNow.Ticks;

        /// <summary>
        ///     Gets a value to be assigned to a property.
        /// </summary>
        /// <param name="entry"> The change tracking entry of the entity for which the value is being generated. </param>
        /// <returns> The value to be assigned to a property. </returns>
        public Guid Next()
        {
            var guidBytes = Guid.NewGuid().ToByteArray();
            var counterBytes = BitConverter.GetBytes(Interlocked.Increment(ref _counter));

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(counterBytes);
            }

            guidBytes[08] = counterBytes[1];
            guidBytes[09] = counterBytes[0];
            guidBytes[10] = counterBytes[7];
            guidBytes[11] = counterBytes[6];
            guidBytes[12] = counterBytes[5];
            guidBytes[13] = counterBytes[4];
            guidBytes[14] = counterBytes[3];
            guidBytes[15] = counterBytes[2];

            return new Guid(guidBytes);
        }

        /// <summary>
        ///     Gets a value indicating whether the values generated are temporary or permanent. This implementation
        ///     always returns false, meaning the generated values will be saved to the database.
        /// </summary>
        public bool GeneratesTemporaryValues => false;
    }
}
