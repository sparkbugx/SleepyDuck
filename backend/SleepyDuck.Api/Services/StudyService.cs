using Microsoft.EntityFrameworkCore;
using SleepyDuck.Api.Data;
using SleepyDuck.Api.Dtos;
using SleepyDuck.Api.Models;

namespace SleepyDuck.Api.Services;

public enum AnswerError
{
    None,
    StepNotFound,
    OptionNotFound,
}

public class StudyService(AppDbContext db, TimeProvider clock)
{
    private DateTime UtcNow => clock.GetUtcNow().UtcDateTime;

    /// <summary>Cards due before the next Guatemala midnight count towards the "today" total.</summary>
    private DateTime EndOfToday => StudyCalendar.EndOfStudyDayUtc(UtcNow);

    public async Task<DashboardStatsDto> GetDashboardStatsAsync(CancellationToken ct)
    {
        var cards = await db.ScenarioCards
            .AsNoTracking()
            .Select(c => new
            {
                c.Category,
                IsNew = c.Progress == null,
                IsMastered = c.Progress != null && c.Progress.MasteryLevel == MasteryLevel.Mastered,
                NextReviewDate = c.Progress == null ? null : c.Progress.NextReviewDate,
            })
            .ToListAsync(ct);

        var endOfToday = EndOfToday;
        var now = UtcNow;

        var categories = cards
            .GroupBy(c => c.Category)
            .OrderBy(g => g.Key)
            .Select(g => new CategoryStatsDto(
                g.Key,
                g.Count(),
                g.Count(c => c.NextReviewDate < endOfToday),
                g.Count(c => c.NextReviewDate <= now),
                g.Count(c => c.IsNew),
                g.Count(c => c.IsMastered)))
            .ToList();

        return new DashboardStatsDto(
            cards.Count,
            categories.Sum(c => c.CardsToReviewToday),
            categories.Sum(c => c.CardsDueNow),
            categories.Sum(c => c.NewCards),
            categories.Sum(c => c.MasteredCards),
            cards.Min(c => c.NextReviewDate),
            categories);
    }

    public async Task<IReadOnlyList<ScenarioSummaryDto>> GetScenariosAsync(CancellationToken ct)
    {
        var cards = await db.ScenarioCards
            .AsNoTracking()
            .AsSplitQuery()
            .Include(c => c.Progress)
            .Include(c => c.Steps)
            .OrderBy(c => c.Category)
            .ThenBy(c => c.Title)
            .ToListAsync(ct);

        return cards
            .Select(c => new ScenarioSummaryDto(c.Id, c.Title, c.Category, c.Steps.Count, ToDto(c.Progress)))
            .ToList();
    }

    public async Task<ScenarioCardDto?> GetScenarioAsync(Guid id, CancellationToken ct)
    {
        var card = await CardsWithSteps().FirstOrDefaultAsync(c => c.Id == id, ct);
        return card is null ? null : ToDto(card);
    }

    /// <summary>Everything due right now, oldest first, followed by up to <paramref name="newLimit"/> new cards.</summary>
    public async Task<IReadOnlyList<ScenarioCardDto>> GetStudyQueueAsync(
        ScenarioCategory? category, int newLimit, CancellationToken ct)
    {
        var cards = CardsWithSteps();
        if (category is not null)
        {
            cards = cards.Where(c => c.Category == category);
        }

        var now = UtcNow;
        var due = await cards
            .Where(c => c.Progress != null && c.Progress.NextReviewDate <= now)
            .OrderBy(c => c.Progress!.NextReviewDate)
            .ToListAsync(ct);

        var fresh = await cards
            .Where(c => c.Progress == null)
            .OrderBy(c => c.Title)
            .Take(newLimit)
            .ToListAsync(ct);

        return due.Concat(fresh).Select(ToDto).ToList();
    }

    public async Task<(AnswerResultDto? Result, AnswerError Error)> SubmitAnswerAsync(
        Guid cardId, Guid stepId, Guid selectedOptionId, CancellationToken ct)
    {
        var step = await db.SequentialSteps
            .AsNoTracking()
            .Include(s => s.ScenarioCard)
            .Include(s => s.Distractors)
            .FirstOrDefaultAsync(s => s.Id == stepId && s.ScenarioCardId == cardId, ct);

        if (step is null)
        {
            return (null, AnswerError.StepNotFound);
        }

        var isCorrect = selectedOptionId == step.CorrectOptionId;
        var distractor = step.Distractors.FirstOrDefault(d => d.Id == selectedOptionId);
        if (!isCorrect && distractor is null)
        {
            return (null, AnswerError.OptionNotFound);
        }

        var technicalExplanation = step.ScenarioCard!.TechnicalExplanation;

        if (!isCorrect)
        {
            // A wrong step ends the card on the spot and resets its interval.
            var progress = await ApplyReviewAsync(cardId, ReviewRating.Again, ct);
            return (new AnswerResultDto(
                false,
                step.CorrectOptionId,
                step.CorrectAction,
                step.Explanation,
                distractor!.Explanation,
                IsScenarioComplete: false,
                technicalExplanation,
                RatingPreviews: null,
                progress), AnswerError.None);
        }

        var lastStep = await db.SequentialSteps
            .Where(s => s.ScenarioCardId == cardId)
            .MaxAsync(s => s.Order, ct);
        var isComplete = step.Order == lastStep;

        return (new AnswerResultDto(
            true,
            step.CorrectOptionId,
            step.CorrectAction,
            step.Explanation,
            DistractorExplanation: null,
            isComplete,
            isComplete ? technicalExplanation : null,
            isComplete ? await PreviewRatingsAsync(cardId, ct) : null,
            Progress: null), AnswerError.None);
    }

    /// <returns>The updated progress, or null if the card doesn't exist.</returns>
    public async Task<UserProgressDto?> SubmitReviewAsync(Guid cardId, ReviewRating rating, CancellationToken ct)
    {
        var exists = await db.ScenarioCards.AnyAsync(c => c.Id == cardId, ct);
        return exists ? await ApplyReviewAsync(cardId, rating, ct) : null;
    }

    private async Task<UserProgressDto> ApplyReviewAsync(Guid cardId, ReviewRating rating, CancellationToken ct)
    {
        var progress = await db.UserProgress.FirstOrDefaultAsync(p => p.ScenarioCardId == cardId, ct);
        if (progress is null)
        {
            progress = new UserProgress { ScenarioCardId = cardId };
            db.UserProgress.Add(progress);
        }

        var now = UtcNow;
        var next = Sm2Scheduler.Schedule(StateOf(progress), rating, now);

        progress.EaseFactor = next.EaseFactor;
        progress.IntervalDays = next.IntervalDays;
        progress.Repetitions = next.Repetitions;
        progress.NextReviewDate = next.NextReviewDate;
        progress.MasteryLevel = next.MasteryLevel;
        progress.LastStudiedDate = now;
        progress.TimesStudied++;
        if (rating == ReviewRating.Again)
        {
            progress.Lapses++;
        }

        await db.SaveChangesAsync(ct);
        return ToDto(progress)!;
    }

    private async Task<IReadOnlyList<RatingPreviewDto>> PreviewRatingsAsync(Guid cardId, CancellationToken ct)
    {
        var progress = await db.UserProgress.AsNoTracking().FirstOrDefaultAsync(p => p.ScenarioCardId == cardId, ct);
        var state = progress is null
            ? new SrsState(Sm2Scheduler.DefaultEase, IntervalDays: 0, Repetitions: 0)
            : StateOf(progress);
        var now = UtcNow;

        return Enum.GetValues<ReviewRating>()
            .Select(rating =>
            {
                var result = Sm2Scheduler.Schedule(state, rating, now);
                return new RatingPreviewDto(rating, result.IntervalDays, result.NextReviewDate);
            })
            .ToList();
    }

    private IQueryable<ScenarioCard> CardsWithSteps() => db.ScenarioCards
        .AsNoTracking()
        .AsSplitQuery()
        .Include(c => c.Progress)
        .Include(c => c.Steps.OrderBy(s => s.Order))
        .ThenInclude(s => s.Distractors);

    private static SrsState StateOf(UserProgress progress) =>
        new(progress.EaseFactor, progress.IntervalDays, progress.Repetitions);

    private static ScenarioCardDto ToDto(ScenarioCard card) => new(
        card.Id,
        card.Title,
        card.Category,
        card.ProblemStatement,
        card.Steps.Select(ToDto).ToList(),
        ToDto(card.Progress));

    private static SequentialStepDto ToDto(SequentialStep step)
    {
        var options = step.Distractors
            .Select(d => new StepOptionDto(d.Id, d.Text))
            .Append(new StepOptionDto(step.CorrectOptionId, step.CorrectAction))
            .OrderBy(_ => Random.Shared.Next())
            .ToList();

        return new SequentialStepDto(step.Id, step.Order, step.Prompt, options);
    }

    private static UserProgressDto? ToDto(UserProgress? p) => p is null
        ? null
        : new UserProgressDto(
            p.ScenarioCardId,
            p.TimesStudied,
            p.MasteryLevel,
            p.LastStudiedDate,
            p.NextReviewDate,
            p.EaseFactor,
            p.IntervalDays,
            p.Repetitions,
            p.Lapses);
}
