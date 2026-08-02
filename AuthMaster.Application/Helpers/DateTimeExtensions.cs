using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Application.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime ToEgyptTime(this DateTime utcDateTime)
        {
            try
            {
                TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, egyptTimeZone);
            }
            catch (TimeZoneNotFoundException)
            {
                 return utcDateTime.AddHours(3);
            }
        }
        public static DateTime GetCurrentEgyptTime()
        {
            return DateTime.UtcNow.ToEgyptTime();
        }
    }
}
