using SleepyDuck.Api.Models;

namespace SleepyDuck.Api.Services;

public sealed record SrsState(double EaseFactor, double IntervalDays, int Repetitions);

public sealed record SrsResult(
    double EaseFactor,
    double IntervalDays,
    int Repetitions,
    DateTime NextReviewDate,
    MasteryLevel MasteryLevel);

/// <summary>
/// SM-2 with a five-button rating. The first two reviews use fixed intervals, and the SM-2
/// multipliers only apply once a card has passed both. An interval only ever shrinks on
/// "Again", which sends a card back to the first review. Pure, so the schedule for each
/// rating can also be previewed on the buttons before the user commits.
/// </summary>
public static class Sm2Scheduler
{
    public const double DefaultEase = 2.5;
    public const double MinimumEase = 1.3;
    public const int MaximumIntervalDays = 365;
    public const int MasteredIntervalDays = 5;
    public const int ReviewingIntervalDays = 2;

    public const double FirstHardIntervalDays = 1.0 / 24;
    public const double FirstGoodIntervalDays = 4.0 / 24;
    public const double FirstEasyIntervalDays = 0.5;
    public const double FirstVeryEasyIntervalDays = 1;

    public const double SecondHardIntervalDays = 4.0 / 24;
    public const double SecondGoodIntervalDays = 0.5;
    public const double SecondEasyIntervalDays = 1;
    public const double SecondVeryEasyIntervalDays = 2;

    public static readonly TimeSpan RelearnDelay = TimeSpan.FromMinutes(10);

    private const double HardIntervalMultiplier = 1.2;
    private const double EasyBonus = 1.3;
    private const double VeryEasyBonus = 1.7;

    public static SrsResult Schedule(SrsState state, ReviewRating rating, DateTime utcNow)
    {
        if (rating == ReviewRating.Again)
        {
            return new SrsResult(
                AdjustEase(state.EaseFactor, -0.20),
                IntervalDays: 0,
                Repetitions: 0,
                utcNow + RelearnDelay,
                MasteryLevel.Learning);
        }

        var ease = rating switch
        {
            ReviewRating.Hard => AdjustEase(state.EaseFactor, -0.15),
            ReviewRating.Good => state.EaseFactor,
            ReviewRating.Easy => AdjustEase(state.EaseFactor, 0.15),
            ReviewRating.VeryEasy => AdjustEase(state.EaseFactor, 0.25),
            _ => throw new ArgumentOutOfRangeException(nameof(rating), rating, null),
        };

        var target = state.Repetitions switch
        {
            0 => FirstInterval(rating),
            1 => SecondInterval(rating),
            _ => GraduatedInterval(state, rating),
        };

        // A passed review never shortens a card. Only "Again" above can do that.
        var interval = Math.Min(Math.Max(target, state.IntervalDays), MaximumIntervalDays);

        // Measured from now, not from a day boundary. Snapping to midnight could make
        // Easy (1 day) come due sooner than Good (12 hours).
        return new SrsResult(
            ease,
            interval,
            state.Repetitions + 1,
            utcNow + TimeSpan.FromDays(interval),
            MasteryFor(interval));
    }

    private static double FirstInterval(ReviewRating rating) => rating switch
    {
        ReviewRating.Hard => FirstHardIntervalDays,
        ReviewRating.Good => FirstGoodIntervalDays,
        ReviewRating.Easy => FirstEasyIntervalDays,
        _ => FirstVeryEasyIntervalDays,
    };

    private static double SecondInterval(ReviewRating rating) => rating switch
    {
        ReviewRating.Hard => SecondHardIntervalDays,
        ReviewRating.Good => SecondGoodIntervalDays,
        ReviewRating.Easy => SecondEasyIntervalDays,
        _ => SecondVeryEasyIntervalDays,
    };

    /// <summary>
    /// Whole days. Hard always passes the previous interval, and each rating is at least a
    /// day longer than the one below it, so short intervals can't round to the same value.
    /// </summary>
    private static double GraduatedInterval(SrsState state, ReviewRating rating)
    {
        var previous = state.IntervalDays;
        var scaled = previous * state.EaseFactor;

        var hard = Math.Max(Math.Floor(previous) + 1, RoundDays(previous * HardIntervalMultiplier));
        var good = Math.Max(hard + 1, RoundDays(scaled));
        var easy = Math.Max(good + 1, RoundDays(scaled * EasyBonus));
        var veryEasy = Math.Max(easy + 1, RoundDays(scaled * VeryEasyBonus));

        return rating switch
        {
            ReviewRating.Hard => hard,
            ReviewRating.Good => good,
            ReviewRating.Easy => easy,
            _ => veryEasy,
        };
    }

    private static MasteryLevel MasteryFor(double intervalDays) => intervalDays switch
    {
        >= MasteredIntervalDays => MasteryLevel.Mastered,
        >= ReviewingIntervalDays => MasteryLevel.Reviewing,
        _ => MasteryLevel.Learning,
    };

    private static double AdjustEase(double ease, double delta) =>
        Math.Max(MinimumEase, Math.Round(ease + delta, 2));

    private static double RoundDays(double days) => Math.Round(days, MidpointRounding.AwayFromZero);
}
