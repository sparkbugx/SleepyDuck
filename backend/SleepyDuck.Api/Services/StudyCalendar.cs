namespace SleepyDuck.Api.Services;

/// <summary>
/// The study day runs from midnight to midnight in Guatemala. Timestamps stay in UTC
/// everywhere else, so this is the one place that needs to know about the time zone.
/// </summary>
public static class StudyCalendar
{
    public const string TimeZoneId = "America/Guatemala";

    private static readonly TimeZoneInfo Zone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);

    /// <summary>The UTC instant of the next Guatemala midnight, which ends the current study day.</summary>
    public static DateTime EndOfStudyDayUtc(DateTime utcNow)
    {
        var today = TimeZoneInfo.ConvertTimeFromUtc(utcNow, Zone).Date;
        var nextMidnight = DateTime.SpecifyKind(today.AddDays(1), DateTimeKind.Unspecified);

        return TimeZoneInfo.ConvertTimeToUtc(nextMidnight, Zone);
    }

    public static DateTime ToStudyTime(DateTime utc) => TimeZoneInfo.ConvertTimeFromUtc(utc, Zone);
}
