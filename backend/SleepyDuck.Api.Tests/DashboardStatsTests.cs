using Microsoft.EntityFrameworkCore;
using SleepyDuck.Api.Data;
using SleepyDuck.Api.Models;
using SleepyDuck.Api.Services;

namespace SleepyDuck.Api.Tests;

/// <summary>Runs the real dashboard and queue queries on a fixed clock, with a card due at a chosen time.</summary>
public class DashboardStatsTests
{
    private sealed class FixedClock(DateTime utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => new(utcNow);
    }

    private static DateTime Utc(int day, int hour, int minute = 0) =>
        new(2026, 3, day, hour, minute, 0, DateTimeKind.Utc);

    private static async Task<StudyService> ServiceWithCardDueAt(DateTime dueUtc, DateTime utcNow)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();

        db.UserProgress.Add(new UserProgress
        {
            ScenarioCardId = SeedData.Cards[0].Id,
            NextReviewDate = dueUtc,
            MasteryLevel = MasteryLevel.Learning,
        });
        await db.SaveChangesAsync();

        return new StudyService(db, new FixedClock(utcNow));
    }

    [Fact]
    public async Task Before_6pm_a_card_due_tonight_counts_as_today()
    {
        // 17:00 in Guatemala, and the card is due at 20:00. UTC midnight falls between them.
        var service = await ServiceWithCardDueAt(dueUtc: Utc(11, 2), utcNow: Utc(10, 23));

        var stats = await service.GetDashboardStatsAsync(default);

        Assert.Equal(1, stats.CardsToReviewToday);
        Assert.Equal(0, stats.CardsDueNow);
    }

    [Fact]
    public async Task After_6pm_a_card_due_tomorrow_does_not_count_as_today()
    {
        // 18:30 in Guatemala on the 10th, and the card is due at 09:00 on the 11th.
        var service = await ServiceWithCardDueAt(dueUtc: Utc(11, 15), utcNow: Utc(11, 0, 30));

        var stats = await service.GetDashboardStatsAsync(default);

        Assert.Equal(0, stats.CardsToReviewToday);
    }

    [Fact]
    public async Task A_card_due_in_the_last_minute_of_the_local_day_counts_as_today()
    {
        var service = await ServiceWithCardDueAt(dueUtc: Utc(11, 5, 59), utcNow: Utc(11, 4));

        Assert.Equal(1, (await service.GetDashboardStatsAsync(default)).CardsToReviewToday);
    }

    [Fact]
    public async Task A_card_due_at_local_midnight_belongs_to_the_next_day()
    {
        var service = await ServiceWithCardDueAt(dueUtc: Utc(11, 6), utcNow: Utc(11, 4));

        Assert.Equal(0, (await service.GetDashboardStatsAsync(default)).CardsToReviewToday);
    }

    [Fact]
    public async Task The_new_day_starts_at_local_midnight()
    {
        // The same card as above, seen one minute into the local day it's due on.
        var service = await ServiceWithCardDueAt(dueUtc: Utc(11, 15), utcNow: Utc(11, 6, 1));

        Assert.Equal(1, (await service.GetDashboardStatsAsync(default)).CardsToReviewToday);
    }

    [Fact]
    public async Task The_queue_serves_a_card_only_once_it_is_due()
    {
        var due = Utc(11, 2);

        var waiting = await ServiceWithCardDueAt(due, utcNow: Utc(11, 1, 59));
        var ready = await ServiceWithCardDueAt(due, utcNow: Utc(11, 2));

        var cardId = SeedData.Cards[0].Id;
        Assert.DoesNotContain(await waiting.GetStudyQueueAsync(null, newLimit: 0, default), c => c.Id == cardId);
        Assert.Contains(await ready.GetStudyQueueAsync(null, newLimit: 0, default), c => c.Id == cardId);
    }
}
