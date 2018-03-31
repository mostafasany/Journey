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
            TimeSpan timesince = DateTime.Now - dt;
            return Format(timesince);
        }

        public static string Format(double seconds)
        {
            TimeSpan timesince = TimeSpan.FromSeconds(seconds);

            string str = timesince.ToString(@"hh\:mm\:ss");

            return str;
        }

        public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> list, int count)
        {
            if (count <= 0)
                yield break;
            var r = new Random();
            int limit = count * 10;
            foreach (T item in list.OrderBy(x => r.Next(0, limit)).Take(count))
                yield return item;
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
                int years = timesince.Days / 365;
                if (timesince.Days % 365 != 0)
                    years += 1;
                return
                    $"{years} {(years == 1 ? AppResource.Date_Year : AppResource.Date_Years)} {AppResource.Date_Ago}";
            }

            if (timesince.Days > 30)
            {
                int months = timesince.Days / 30;
                if (timesince.Days % 31 != 0)
                    months += 1;
                return
                    $"{months} {(months == 1 ? AppResource.Date_Month : AppResource.Date_Months)}";
            }

            if (timesince.Days > 0)
                return
                    $"{timesince.Days} {(timesince.Days == 1 ? AppResource.Date_Day : AppResource.Date_Days)}";
            if (timesince.Hours > 0)
                return
                    $"{timesince.Hours} {(timesince.Hours == 1 ? AppResource.Date_Hour : AppResource.Date_Hours)}";
            if (timesince.Minutes > 0)
                return
                    $"{timesince.Minutes} {(timesince.Minutes == 1 ? AppResource.Date_Minute : AppResource.Date_Minutes)}";
            if (timesince.Seconds > 5)
                return $"{timesince.Seconds} {AppResource.Date_Seconds}";
            return AppResource.Date_JustNow;
        }
    }
}