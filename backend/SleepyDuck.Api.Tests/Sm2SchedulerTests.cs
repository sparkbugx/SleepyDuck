using SleepyDuck.Api.Models;
using SleepyDuck.Api.Services;

namespace SleepyDuck.Api.Tests;

public class Sm2SchedulerTests
{
    private static readonly DateTime Now = new(2026, 3, 10, 21, 30, 0, DateTimeKind.Utc);
    private static readonly SrsState NewCard = new(Sm2Scheduler.DefaultEase, IntervalDays: 0, Repetitions: 0);

    [Theory]
    [InlineData(ReviewRating.Hard, 1)]
    [InlineData(ReviewRating.Good, 1)]
    [InlineData(ReviewRating.Easy, 4)]
    public void New_card_gets_the_first_interval_for_its_rating(ReviewRating rating, int expectedDays)
    {
        var result = Sm2Scheduler.Schedule(NewCard, rating, Now);

        Assert.Equal(expectedDays, result.IntervalDays);
        Assert.Equal(1, result.Repetitions);
    }

    [Fact]
    public void Good_follows_the_sm2_sequence()
    {
        var state = NewCard;
        var intervals = new List<int>();

        for (var i = 0; i < 4; i++)
        {
            var result = Sm2Scheduler.Schedule(state, ReviewRating.Good, Now);
            intervals.Add(result.IntervalDays);
            state = new SrsState(result.EaseFactor, result.IntervalDays, result.Repetitions);
        }

        Assert.Equal([1, 6, 15, 38], intervals);
    }

    [Fact]
    public void Again_resets_the_interval_and_returns_the_card_within_minutes()
    {
        var state = new SrsState(2.5, IntervalDays: 40, Repetitions: 5);

        var result = Sm2Scheduler.Schedule(state, ReviewRating.Again, Now);

        Assert.Equal(0, result.IntervalDays);
        Assert.Equal(0, result.Repetitions);
        Assert.Equal(2.3, result.EaseFactor);
        Assert.Equal(Now + Sm2Scheduler.RelearnDelay, result.NextReviewDate);
        Assert.Equal(MasteryLevel.Learning, result.MasteryLevel);
    }

    [Fact]
    public void Ease_never_drops_below_the_minimum()
    {
        var state = new SrsState(1.35, IntervalDays: 10, Repetitions: 3);

        Assert.Equal(Sm2Scheduler.MinimumEase, Sm2Scheduler.Schedule(state, ReviewRating.Again, Now).EaseFactor);
        Assert.Equal(Sm2Scheduler.MinimumEase, Sm2Scheduler.Schedule(state, ReviewRating.Hard, Now).EaseFactor);
    }

    [Fact]
    public void Ratings_are_ordered_from_shortest_to_longest_interval()
    {
        var state = new SrsState(2.5, IntervalDays: 10, Repetitions: 3);

        var hard = Sm2Scheduler.Schedule(state, ReviewRating.Hard, Now).IntervalDays;
        var good = Sm2Scheduler.Schedule(state, ReviewRating.Good, Now).IntervalDays;
        var easy = Sm2Scheduler.Schedule(state, ReviewRating.Easy, Now).IntervalDays;

        Assert.True(hard < good && good < easy, $"hard={hard} good={good} easy={easy}");
    }

    [Fact]
    public void Day_intervals_land_on_utc_midnight()
    {
        var result = Sm2Scheduler.Schedule(NewCard, ReviewRating.Good, Now);

        Assert.Equal(new DateTime(2026, 3, 11, 0, 0, 0, DateTimeKind.Utc), result.NextReviewDate);
    }

    [Fact]
    public void Intervals_are_capped()
    {
        var state = new SrsState(2.5, IntervalDays: 300, Repetitions: 8);

        Assert.Equal(Sm2Scheduler.MaximumIntervalDays, Sm2Scheduler.Schedule(state, ReviewRating.Easy, Now).IntervalDays);
    }

    [Theory]
    [InlineData(4, 5, MasteryLevel.Learning)]
    [InlineData(5, 6, MasteryLevel.Reviewing)]
    [InlineData(17, 20, MasteryLevel.Reviewing)]
    [InlineData(18, 22, MasteryLevel.Mastered)]
    public void Mastery_follows_the_interval(int previousInterval, int expectedInterval, MasteryLevel expected)
    {
        var state = new SrsState(2.5, previousInterval, Repetitions: 3);

        var result = Sm2Scheduler.Schedule(state, ReviewRating.Hard, Now);

        Assert.Equal(expectedInterval, result.IntervalDays);
        Assert.Equal(expected, result.MasteryLevel);
    }
}
