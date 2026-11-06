using System.ComponentModel.DataAnnotations;
using SleepyDuck.Api.Models;

namespace SleepyDuck.Api.Dtos;

// Each of these has a matching interface in frontend/src/app/core/models/api.models.ts.

public sealed record StepOptionDto(Guid Id, string Text);

/// <summary>A step as the client sees it. Options are shuffled and never reveal the answer.</summary>
public sealed record SequentialStepDto(
    Guid Id,
    int Order,
    string Prompt,
    IReadOnlyList<StepOptionDto> Options);

public sealed record UserProgressDto(
    Guid ScenarioCardId,
    int TimesStudied,
    MasteryLevel MasteryLevel,
    DateTime? LastStudiedDate,
    DateTime? NextReviewDate,
    double EaseFactor,
    int IntervalDays,
    int Repetitions,
    int Lapses);

public sealed record ScenarioSummaryDto(
    Guid Id,
    string Title,
    ScenarioCategory Category,
    int StepCount,
    UserProgressDto? Progress);

public sealed record ScenarioCardDto(
    Guid Id,
    string Title,
    ScenarioCategory Category,
    string ProblemStatement,
    IReadOnlyList<SequentialStepDto> Steps,
    UserProgressDto? Progress);

public sealed record SubmitAnswerRequest([Required] Guid? SelectedOptionId);

public sealed record RatingPreviewDto(ReviewRating Rating, int IntervalDays, DateTime NextReviewDate);

/// <param name="IsCorrect">Whether the selected option was the right one.</param>
/// <param name="CorrectOptionId">The right option for this step.</param>
/// <param name="CorrectAction">Text of the right option.</param>
/// <param name="StepExplanation">Why the correct action is right.</param>
/// <param name="DistractorExplanation">Why the chosen distractor is wrong. Null on a correct answer.</param>
/// <param name="IsScenarioComplete">True once the last step has been cleared.</param>
/// <param name="TechnicalExplanation">Null until the card has ended, by success or failure.</param>
/// <param name="RatingPreviews">The schedule for each rating. Null unless every step has been cleared.</param>
/// <param name="Progress">The "Again" progress applied on failure. Null otherwise.</param>
public sealed record AnswerResultDto(
    bool IsCorrect,
    Guid CorrectOptionId,
    string CorrectAction,
    string StepExplanation,
    string? DistractorExplanation,
    bool IsScenarioComplete,
    string? TechnicalExplanation,
    IReadOnlyList<RatingPreviewDto>? RatingPreviews,
    UserProgressDto? Progress);

public sealed record SubmitReviewRequest([Required] ReviewRating? Rating);

public sealed record CategoryStatsDto(
    ScenarioCategory Category,
    int TotalCards,
    int CardsToReviewToday,
    int NewCards,
    int MasteredCards);

public sealed record DashboardStatsDto(
    int TotalCards,
    int CardsToReviewToday,
    int NewCards,
    int MasteredCards,
    DateTime? NextReviewDate,
    IReadOnlyList<CategoryStatsDto> Categories);
