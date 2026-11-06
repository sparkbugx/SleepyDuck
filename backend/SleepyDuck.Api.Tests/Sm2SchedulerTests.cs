using SleepyDuck.Api.Models;
using SleepyDuck.Api.Services;

namespace SleepyDuck.Api.Tests;

public class Sm2SchedulerTests
{
    private const double OneHourInDays = 1.0 / 24;
    private const double FourHoursInDays = 4.0 / 24;

    private static readonly DateTime Now = new(2026, 3, 10, 21, 30, 0, DateTimeKind.Utc);
    private static readonly SrsState NewCard = new(Sm2Scheduler.DefaultEase, IntervalDays: 0, Repetitions: 0);

    [Fact]
    public void VeryEasy_is_the_fifth_rating()
    {
        Assert.Equal(
            [ReviewRating.Again, ReviewRating.Hard, ReviewRating.Good, ReviewRating.Easy, ReviewRating.VeryEasy],
            Enum.GetValues<ReviewRating>());
    }

    [Theory]
    [InlineData(ReviewRating.Hard, OneHourInDays)]
    [InlineData(ReviewRating.Good, FourHoursInDays)]
    [InlineData(ReviewRating.Easy, 0.5)]
    [InlineData(ReviewRating.VeryEasy, 1.0)]
    public void New_card_gets_the_first_interval_for_its_rating(ReviewRating rating, double expectedDays)
    {
        var result = Sm2Scheduler.Schedule(NewCard, rating, Now);

        Assert.Equal(expectedDays, result.IntervalDays);
        Assert.Equal(1, result.Repetitions);
    }

    [Theory]
    [InlineData(ReviewRating.Hard, 1)]
    [InlineData(ReviewRating.Good, 4)]
    [InlineData(ReviewRating.Easy, 12)]
    [InlineData(ReviewRating.VeryEasy, 24)]
    public void New_card_comes_due_exactly_one_interval_from_now(ReviewRating rating, int expectedHours)
    {
        var result = Sm2Scheduler.Schedule(NewCard, rating, Now);

        Assert.Equal(Now.AddHours(expectedHours), result.NextReviewDate, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void New_card_ratings_come_due_in_order_at_any_time_of_day()
    {
        for (var hour = 0; hour < 24; hour++)
        {
            var now = new DateTime(2026, 3, 10, hour, 15, 0, DateTimeKind.Utc);

            var dueDates = Enum.GetValues<ReviewRating>()
                .Select(rating => Sm2Scheduler.Schedule(NewCard, rating, now).NextReviewDate)
                .ToList();

            Assert.Equal(dueDates.Order(), dueDates);
            Assert.Equal(dueDates.Count, dueDates.Distinct().Count());
        }
    }

    [Fact]
    public void Good_follows_the_sm2_sequence()
    {
        var state = NewCard;
        var intervals = new List<double>();

        for (var i = 0; i < 4; i++)
        {
            var result = Sm2Scheduler.Schedule(state, ReviewRating.Good, Now);
            intervals.Add(result.IntervalDays);
            state = new SrsState(result.EaseFactor, result.IntervalDays, result.Repetitions);
        }

        Assert.Equal([FourHoursInDays, 0.5, 2, 5], intervals);
    }

    [Fact]
    public void Repeated_Hard_keeps_moving_forward()
    {
        var state = NewCard;
        var intervals = new List<double>();

        for (var i = 0; i < 6; i++)
        {
            var result = Sm2Scheduler.Schedule(state, ReviewRating.Hard, Now);
            intervals.Add(result.IntervalDays);
            state = new SrsState(result.EaseFactor, result.IntervalDays, result.Repetitions);
        }

        Assert.Equal([OneHourInDays, FourHoursInDays, 1, 2, 3, 4], intervals);
    }

    [Theory]
    [InlineData(ReviewRating.Hard, FourHoursInDays)]
    [InlineData(ReviewRating.Good, 0.5)]
    [InlineData(ReviewRating.Easy, 1.0)]
    [InlineData(ReviewRating.VeryEasy, 2.0)]
    public void Second_review_uses_the_fixed_interval_when_it_is_not_shorter(ReviewRating rating, double expectedDays)
    {
        foreach (var previous in new[] { OneHourInDays, FourHoursInDays })
        {
            foreach (var ease in new[] { 1.3, 2.5, 2.75 })
            {
                var result = Sm2Scheduler.Schedule(new SrsState(ease, previous, Repetitions: 1), rating, Now);

                Assert.Equal(expectedDays, result.IntervalDays);
                Assert.Equal(2, result.Repetitions);
            }
        }
    }

    [Theory]
    [InlineData(0.5, ReviewRating.Hard, 0.5)]
    [InlineData(0.5, ReviewRating.Good, 0.5)]
    [InlineData(0.5, ReviewRating.Easy, 1.0)]
    [InlineData(0.5, ReviewRating.VeryEasy, 2.0)]
    [InlineData(1.0, ReviewRating.Hard, 1.0)]
    [InlineData(1.0, ReviewRating.Good, 1.0)]
    [InlineData(1.0, ReviewRating.Easy, 1.0)]
    [InlineData(1.0, ReviewRating.VeryEasy, 2.0)]
    public void Second_review_keeps_a_longer_previous_interval(double previous, ReviewRating rating, double expectedDays)
    {
        var result = Sm2Scheduler.Schedule(new SrsState(2.5, previous, Repetitions: 1), rating, Now);

        Assert.Equal(expectedDays, result.IntervalDays);
        Assert.Equal(Now.AddDays(expectedDays), result.NextReviewDate, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void A_successful_review_never_shrinks_the_interval()
    {
        var passes = new[] { ReviewRating.Hard, ReviewRating.Good, ReviewRating.Easy, ReviewRating.VeryEasy };
        var sequences = 0;

        void Walk(SrsState state, int remaining)
        {
            if (remaining == 0)
            {
                sequences++;
                return;
            }

            foreach (var rating in passes)
            {
                var next = Sm2Scheduler.Schedule(state, rating, Now);

                Assert.True(
                    next.IntervalDays >= state.IntervalDays,
                    $"{rating} shrank {state.IntervalDays} to {next.IntervalDays} at repetition {state.Repetitions}");

                Walk(new SrsState(next.EaseFactor, next.IntervalDays, next.Repetitions), remaining - 1);
            }
        }

        Walk(NewCard, remaining: 7);

        Assert.Equal(16384, sequences);
    }

    [Theory]
    [InlineData(3.0, 1)]
    [InlineData(40.0, 5)]
    public void Only_Again_can_shrink_the_interval(double previous, int repetitions)
    {
        var result = Sm2Scheduler.Schedule(new SrsState(2.5, previous, repetitions), ReviewRating.Again, Now);

        Assert.Equal(0, result.IntervalDays);
    }

    [Fact]
    public void Again_on_the_second_review_returns_to_the_first_review()
    {
        var result = Sm2Scheduler.Schedule(new SrsState(2.5, 0.5, Repetitions: 1), ReviewRating.Again, Now);

        Assert.Equal(0, result.IntervalDays);
        Assert.Equal(0, result.Repetitions);
        Assert.Equal(Now + Sm2Scheduler.RelearnDelay, result.NextReviewDate);
    }

    [Theory]
    [InlineData(ReviewRating.Hard, 3.0)]
    [InlineData(ReviewRating.Good, 5.0)]
    [InlineData(ReviewRating.Easy, 7.0)]
    [InlineData(ReviewRating.VeryEasy, 9.0)]
    public void Multipliers_start_on_the_third_review(ReviewRating rating, double expectedDays)
    {
        var result = Sm2Scheduler.Schedule(new SrsState(2.5, 2, Repetitions: 2), rating, Now);

        Assert.Equal(expectedDays, result.IntervalDays);
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
    public void A_reset_card_starts_again_on_the_new_card_intervals()
    {
        var reset = Sm2Scheduler.Schedule(new SrsState(2.5, 40, 5), ReviewRating.Again, Now);
        var state = new SrsState(reset.EaseFactor, reset.IntervalDays, reset.Repetitions);

        Assert.Equal(OneHourInDays, Sm2Scheduler.Schedule(state, ReviewRating.Hard, Now).IntervalDays);
        Assert.Equal(FourHoursInDays, Sm2Scheduler.Schedule(state, ReviewRating.Good, Now).IntervalDays);
    }

    [Fact]
    public void Ease_never_drops_below_the_minimum()
    {
        var state = new SrsState(1.35, IntervalDays: 10, Repetitions: 3);

        Assert.Equal(Sm2Scheduler.MinimumEase, Sm2Scheduler.Schedule(state, ReviewRating.Again, Now).EaseFactor);
        Assert.Equal(Sm2Scheduler.MinimumEase, Sm2Scheduler.Schedule(state, ReviewRating.Hard, Now).EaseFactor);
    }

    [Fact]
    public void VeryEasy_raises_ease_more_than_Easy()
    {
        var easy = Sm2Scheduler.Schedule(NewCard, ReviewRating.Easy, Now);
        var veryEasy = Sm2Scheduler.Schedule(NewCard, ReviewRating.VeryEasy, Now);

        Assert.Equal(2.65, easy.EaseFactor);
        Assert.Equal(2.75, veryEasy.EaseFactor);
    }

    [Theory]
    [InlineData(0.0, 0, 2.5)]
    [InlineData(OneHourInDays, 1, 2.5)]
    [InlineData(FourHoursInDays, 1, 2.5)]
    [InlineData(0.5, 2, 2.2)]
    [InlineData(0.5, 2, 1.3)]
    [InlineData(1.0, 3, 1.3)]
    [InlineData(2.0, 3, 2.5)]
    [InlineData(10.0, 3, 2.5)]
    public void Ratings_get_longer_from_Hard_to_VeryEasy(double intervalDays, int repetitions, double ease)
    {
        var state = new SrsState(ease, intervalDays, repetitions);

        var hard = Sm2Scheduler.Schedule(state, ReviewRating.Hard, Now).IntervalDays;
        var good = Sm2Scheduler.Schedule(state, ReviewRating.Good, Now).IntervalDays;
        var easy = Sm2Scheduler.Schedule(state, ReviewRating.Easy, Now).IntervalDays;
        var veryEasy = Sm2Scheduler.Schedule(state, ReviewRating.VeryEasy, Now).IntervalDays;

        Assert.True(
            hard < good && good < easy && easy < veryEasy,
            $"hard={hard} good={good} easy={easy} veryEasy={veryEasy}");
    }

    [Fact]
    public void Third_review_moves_a_sub_day_interval_to_whole_days()
    {
        var state = new SrsState(2.5, IntervalDays: 0.5, Repetitions: 2);

        foreach (var rating in new[] { ReviewRating.Hard, ReviewRating.Good, ReviewRating.Easy, ReviewRating.VeryEasy })
        {
            var interval = Sm2Scheduler.Schedule(state, rating, Now).IntervalDays;

            Assert.True(interval >= 1, $"{rating} gave {interval}");
            Assert.Equal(Math.Floor(interval), interval);
        }
    }

    [Fact]
    public void Intervals_are_capped()
    {
        var state = new SrsState(2.5, IntervalDays: 300, Repetitions: 8);

        Assert.Equal(Sm2Scheduler.MaximumIntervalDays, Sm2Scheduler.Schedule(state, ReviewRating.Easy, Now).IntervalDays);
        Assert.Equal(Sm2Scheduler.MaximumIntervalDays, Sm2Scheduler.Schedule(state, ReviewRating.VeryEasy, Now).IntervalDays);
    }

    [Theory]
    [InlineData(0.5, 1, MasteryLevel.Learning)]
    [InlineData(1, 2, MasteryLevel.Reviewing)]
    [InlineData(3, 4, MasteryLevel.Reviewing)]
    [InlineData(4, 5, MasteryLevel.Mastered)]
    public void Mastery_follows_the_interval(double previousInterval, double expectedInterval, MasteryLevel expected)
    {
        var state = new SrsState(2.5, previousInterval, Repetitions: 3);

        var result = Sm2Scheduler.Schedule(state, ReviewRating.Hard, Now);

        Assert.Equal(expectedInterval, result.IntervalDays);
        Assert.Equal(expected, result.MasteryLevel);
    }

    [Fact]
    public void Always_pressing_Good_masters_a_card_on_the_fourth_review()
    {
        var state = NewCard;
        var levels = new List<MasteryLevel>();

        for (var review = 0; review < 4; review++)
        {
            var result = Sm2Scheduler.Schedule(state, ReviewRating.Good, Now);
            levels.Add(result.MasteryLevel);
            state = new SrsState(result.EaseFactor, result.IntervalDays, result.Repetitions);
        }

        Assert.Equal(
            [MasteryLevel.Learning, MasteryLevel.Learning, MasteryLevel.Reviewing, MasteryLevel.Mastered],
            levels);
    }

    [Fact]
    public void Sub_day_intervals_stay_in_Learning()
    {
        Assert.Equal(MasteryLevel.Learning, Sm2Scheduler.Schedule(NewCard, ReviewRating.Hard, Now).MasteryLevel);
        Assert.Equal(MasteryLevel.Learning, Sm2Scheduler.Schedule(NewCard, ReviewRating.Good, Now).MasteryLevel);
    }
}
