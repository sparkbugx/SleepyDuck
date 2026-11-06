using SleepyDuck.Api.Services;

namespace SleepyDuck.Api.Models;

/// <summary>
/// SRS state for a card. Created the first time a card is studied, so a card with no
/// row is "new". The app has no accounts yet, so there is one row per card.
/// </summary>
public class UserProgress
{
    public Guid Id { get; set; }
    public Guid ScenarioCardId { get; set; }

    public int TimesStudied { get; set; }
    public MasteryLevel MasteryLevel { get; set; } = MasteryLevel.New;
    public DateTime? LastStudiedDate { get; set; }
    public DateTime? NextReviewDate { get; set; }

    public double EaseFactor { get; set; } = Sm2Scheduler.DefaultEase;
    public int IntervalDays { get; set; }
    public int Repetitions { get; set; }
    public int Lapses { get; set; }

    public ScenarioCard? ScenarioCard { get; set; }
}
