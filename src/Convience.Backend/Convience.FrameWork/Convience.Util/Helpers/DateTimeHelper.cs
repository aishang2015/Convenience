using System;

namespace Convience.Util.Helpers
{
    public static class DateTimeHelper
    {
        private static DateTime _startedDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        /// 日期转时间戳
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="milliseconds">true：13位，false：10位</param>
        /// <returns>时间戳</returns>
        public static string DateTime2TimeStamp(DateTime dateTime, bool milliseconds = false)
        {
            var ts = dateTime - _startedDate;
            return Convert.ToInt64(milliseconds ? ts.TotalMilliseconds : ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 时间戳转日期
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <param name="milliseconds">true：13位，false：10位</param>
        /// <returns>日期</returns>
        public static DateTime TimeStamp2DateTime(long timestamp, bool milliseconds = false)
        {
            return milliseconds ? _startedDate.AddTicks(timestamp * 10000000) : _startedDate.AddTicks(timestamp * 10000);
        }

        public static string WeekDay(DateTime dateTime)
        {
            return dateTime.DayOfWeek switch
            {
                DayOfWeek.Sunday => "星期日",
                DayOfWeek.Monday => "星期一",
                DayOfWeek.Tuesday => "星期二",
                DayOfWeek.Wednesday => "星期三",
                DayOfWeek.Thursday => "星期四",
                DayOfWeek.Friday => "星期五",
                DayOfWeek.Saturday => "星期六",
                _ => ""
            };
        }
    }
}
