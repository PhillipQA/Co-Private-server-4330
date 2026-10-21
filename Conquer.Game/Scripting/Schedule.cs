namespace Conquer.Game.Scripting;

public static class Schedule
{
    private const int MinutesPerDay = 24 * 60;

    extension(DateTime now)
    {
        public bool IsBetween(DateTime first, DateTime last) => now >= first && now <= last;

        public bool IsYearly(int firstMonth, int firstDay, TimeOnly first, int lastMonth, int lastDay, TimeOnly last) =>
            now.IsBetween(On(now.Year, firstMonth, firstDay, first), On(now.Year, lastMonth, lastDay, last));

        public bool IsMonthly(int firstDay, TimeOnly first, int lastDay, TimeOnly last) =>
            now.IsBetween(On(now.Year, now.Month, firstDay, first), On(now.Year, now.Month, lastDay, last));

        public bool IsWeekly(DayOfWeek firstDay, TimeOnly first, DayOfWeek lastDay, TimeOnly last) =>
            Within((int)now.DayOfWeek * MinutesPerDay + Minutes(now), (int)firstDay * MinutesPerDay + Minutes(first),
                (int)lastDay * MinutesPerDay + Minutes(last));

        public bool IsDaily(TimeOnly first, TimeOnly last) => Within(Minutes(now), Minutes(first), Minutes(last));

        public bool IsHourly(int firstMinute, int lastMinute) => Within(now.Minute, firstMinute, lastMinute);
    }

    private static DateTime On(int year, int month, int day, TimeOnly time) =>
        new DateTime(year, 1, 1).AddMonths(month - 1).AddDays(day - 1) + time.ToTimeSpan();

    private static int Minutes(DateTime time) => time.Hour * 60 + time.Minute;

    private static int Minutes(TimeOnly time) => time.Hour * 60 + time.Minute;

    private static bool Within(int value, int first, int last) => value >= first && value <= last;
}
