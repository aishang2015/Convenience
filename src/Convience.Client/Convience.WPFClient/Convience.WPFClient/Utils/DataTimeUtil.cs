using System;

namespace Convience.WPFClient.Utils
{
    public static class DataTimeUtil
    {

        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            var intStamp = long.Parse(timeStamp);
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = intStamp * 10000000;
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}
