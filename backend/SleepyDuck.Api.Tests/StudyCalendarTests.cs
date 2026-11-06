using SleepyDuck.Api.Models;
using SleepyDuck.Api.Services;

namespace SleepyDuck.Api.Tests;

public class StudyCalendarTests
{
    private static DateTime Utc(int day, int hour, int minute = 0) =>
        new(2026, 3, day, hour, minute, 0, DateTimeKind.Utc);

    [Theory]
    [InlineData(10, 12, 0, 11, 6)] // 06:00 in Guatemala on the 10th
    [InlineData(10, 23, 59, 11, 6)] // 17:59, a minute before UTC midnight
    [InlineData(11, 0, 0, 11, 6)] // 18:00, exactly UTC midnight, still the 10th locally
    [InlineData(11, 5, 59, 11, 6)] // 23:59, the last minute of the 10th locally
    [InlineData(11, 6, 0, 12, 6)] // 00:00, the 11th begins
    public void The_study_day_ends_at_Guatemala_midnight(
        int nowDay, int nowHour, int nowMinute, int endDay, int endHour)
    {
        Assert.Equal(Utc(endDay, endHour), StudyCalendar.EndOfStudyDayUtc(Utc(nowDay, nowHour, nowMinute)));
    }

    [Fact]
    public void The_end_of_the_study_day_is_a_utc_value()
    {
        // Npgsql rejects any other kind on a timestamptz column.
        Assert.Equal(DateTimeKind.Utc, StudyCalendar.EndOfStudyDayUtc(Utc(10, 12)).Kind);
    }

    [Fact]
    public void Guatemala_stays_six_hours_behind_utc_all_year()
    {
        var january = new DateTime(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        var july = new DateTime(2026, 7, 15, 12, 0, 0, DateTimeKind.Utc);

        Assert.Equal(6, StudyCalendar.ToStudyTime(january).Hour);
        Assert.Equal(6, StudyCalendar.ToStudyTime(july).Hour);
    }

    [Fact]
    public void A_one_day_interval_lands_at_the_same_local_time_the_next_day()
    {
        var sevenPmInGuatemala = Utc(11, 1);
        var card = new SrsState(2.5, IntervalDays: 0, Repetitions: 0);

        var result = Sm2Scheduler.Schedule(card, ReviewRating.VeryEasy, sevenPmInGuatemala);

        Assert.Equal(new DateTime(2026, 3, 11, 19, 0, 0), StudyCalendar.ToStudyTime(result.NextReviewDate));
    }

    [Fact]
    public void A_longer_interval_is_never_due_before_a_shorter_one()
    {
        var justBeforeMidnightInGuatemala = Utc(11, 5, 30);
        var card = new SrsState(2.5, IntervalDays: 0, Repetitions: 0);

        var good = Sm2Scheduler.Schedule(card, ReviewRating.Good, justBeforeMidnightInGuatemala);
        var veryEasy = Sm2Scheduler.Schedule(card, ReviewRating.VeryEasy, justBeforeMidnightInGuatemala);

        Assert.True(good.NextReviewDate < veryEasy.NextReviewDate);
    }
}
