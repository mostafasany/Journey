using System;
using System.Collections.Generic;
using System.Linq;
using Journey.Resources;

namespace Journey.Models.Post
{
    public static class DateHelper
    {
        public static string Format(DateTime dt)
        {
            var timesince = DateTime.Now - dt;
            return Format(timesince);
        }

        public static string GiveMeADateTime(this DateTime date)
        {
            if (date == DateTime.Now)
                return AppResource.Date_Today;
            if (date == DateTime.Now.AddDays(1))
                return AppResource.Date_Tomorrow;
            return string.Format("({0}) {1}", date.DayOfWeek.ToString().Substring(0, 3), date.ToString("dd/M/yy"));
        }

        private static string Format(TimeSpan timesince)
        {
            if (timesince.Days > 365)
            {
                var years = timesince.Days / 365;
                if (timesince.Days % 365 != 0)
                    years += 1;
                return $"{years} {(years == 1 ? AppResource.Date_Year : AppResource.Date_Years)} {AppResource.Date_Ago}";
            }
            if (timesince.Days > 30)
            {
                var months = timesince.Days / 30;
                if (timesince.Days % 31 != 0)
                    months += 1;
                return $"{months} {(months == 1 ? AppResource.Date_Month : AppResource.Date_Months)} {AppResource.Date_Ago}";
            }
            if (timesince.Days > 0)
                return $"{timesince.Days} {(timesince.Days == 1 ? AppResource.Date_Day : AppResource.Date_Days)} {AppResource.Date_Ago}";
            if (timesince.Hours > 0)
                return $"{timesince.Hours} {(timesince.Hours == 1 ? AppResource.Date_Hour : AppResource.Date_Hours)} {AppResource.Date_Ago}";
            if (timesince.Minutes > 0)
                return $"{timesince.Minutes} {(timesince.Minutes == 1 ? AppResource.Date_Minute : AppResource.Date_Minutes)} {AppResource.Date_Ago}";
            if (timesince.Seconds > 5)
                return $"{timesince.Seconds} {AppResource.Date_Seconds} {AppResource.Date_Ago}";
            return AppResource.Date_JustNow;
        }

        public static string Format(double seconds)
        {
            var timesince = TimeSpan.FromSeconds(seconds);

            var str = timesince.ToString(@"hh\:mm\:ss");

            return str;
        }

        public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> list, int count)
        {
            if (count <= 0)
                yield break;
            var r = new Random();
            var limit = count * 10;
            foreach (var item in list.OrderBy(x => r.Next(0, limit)).Take(count))
                yield return item;
        }
    }
}