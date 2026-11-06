using SleepyDuck.Api.Models;

namespace SleepyDuck.Api.Services;

public sealed record SrsState(double EaseFactor, int IntervalDays, int Repetitions);

public sealed record SrsResult(
    double EaseFactor,
    int IntervalDays,
    int Repetitions,
    DateTime NextReviewDate,
    MasteryLevel MasteryLevel);

/// <summary>
/// SM-2 with Anki's four-button rating. Pure, so the schedule for each rating can also
/// be previewed on the buttons before the user commits.
/// </summary>
public static class Sm2Scheduler
{
    public const double DefaultEase = 2.5;
    public const double MinimumEase = 1.3;
    public const int MaximumIntervalDays = 365;
    public const int MasteredIntervalDays = 21;
    public const int ReviewingIntervalDays = 6;

    public static readonly TimeSpan RelearnDelay = TimeSpan.FromMinutes(10);

    private const double HardIntervalMultiplier = 1.2;
    private const double EasyBonus = 1.3;

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

        var (interval, ease) = rating switch
        {
            ReviewRating.Hard => (HardInterval(state), AdjustEase(state.EaseFactor, -0.15)),
            ReviewRating.Good => (GoodInterval(state), state.EaseFactor),
            ReviewRating.Easy => (EasyInterval(state), AdjustEase(state.EaseFactor, 0.15)),
            _ => throw new ArgumentOutOfRangeException(nameof(rating), rating, null),
        };

        interval = Math.Min(interval, MaximumIntervalDays);

        // Day intervals land on a UTC midnight, so a card reviewed late in the evening
        // is due the following morning rather than the following evening.
        return new SrsResult(
            ease,
            interval,
            state.Repetitions + 1,
            utcNow.Date.AddDays(interval),
            MasteryFor(interval));
    }

    private static int HardInterval(SrsState state) =>
        state.Repetitions == 0
            ? 1
            : Math.Max(state.IntervalDays + 1, Round(state.IntervalDays * HardIntervalMultiplier));

    private static int GoodInterval(SrsState state) => state.Repetitions switch
    {
        0 => 1,
        1 => ReviewingIntervalDays,
        _ => Math.Max(state.IntervalDays + 1, Round(state.IntervalDays * state.EaseFactor)),
    };

    private static int EasyInterval(SrsState state) => state.Repetitions switch
    {
        0 => 4,
        1 => Round(Math.Max(state.IntervalDays, ReviewingIntervalDays) * EasyBonus),
        _ => Math.Max(state.IntervalDays + 1, Round(state.IntervalDays * state.EaseFactor * EasyBonus)),
    };

    private static MasteryLevel MasteryFor(int intervalDays) => intervalDays switch
    {
        >= MasteredIntervalDays => MasteryLevel.Mastered,
        >= ReviewingIntervalDays => MasteryLevel.Reviewing,
        _ => MasteryLevel.Learning,
    };

    private static double AdjustEase(double ease, double delta) =>
        Math.Max(MinimumEase, Math.Round(ease + delta, 2));

    private static int Round(double days) => (int)Math.Round(days, MidpointRounding.AwayFromZero);
}
