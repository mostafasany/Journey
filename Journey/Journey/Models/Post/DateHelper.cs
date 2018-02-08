using System;
using System.Collections.Generic;
using System.Linq;

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
                return "Today";
            if (date == DateTime.Now.AddDays(1))
                return "Tomorrow";
            return string.Format("({0}) {1}", date.DayOfWeek.ToString().Substring(0, 3), date.ToString("dd/M/yy"));
        }

        private static string Format(TimeSpan timesince)
        {
            if (timesince.Days > 365)
            {
                var years = timesince.Days / 365;
                if (timesince.Days % 365 != 0)
                    years += 1;
                return $"{years} {(years == 1 ? "year" : "years")} ago";
            }
            if (timesince.Days > 30)
            {
                var months = timesince.Days / 30;
                if (timesince.Days % 31 != 0)
                    months += 1;
                return $"{months} {(months == 1 ? "month" : "months")} ago";
            }
            if (timesince.Days > 0)
                return $"{timesince.Days} {(timesince.Days == 1 ? "day" : "days")} ago";
            if (timesince.Hours > 0)
                return $"{timesince.Hours} {(timesince.Hours == 1 ? "hour" : "hours")} ago";
            if (timesince.Minutes > 0)
                return $"{timesince.Minutes} {(timesince.Minutes == 1 ? "minute" : "minutes")} ago";
            if (timesince.Seconds > 5)
                return $"{timesince.Seconds} seconds ago";
            return "just now";
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