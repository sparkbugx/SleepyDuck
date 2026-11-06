using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SleepyDuck.Api.Dtos;
using SleepyDuck.Api.Models;
using SleepyDuck.Api.Services;

namespace SleepyDuck.Api.Controllers;

[ApiController]
[Route("api/scenarios")]
public class ScenariosController(StudyService study) : ControllerBase
{
    [HttpGet]
    public Task<IReadOnlyList<ScenarioSummaryDto>> GetAll(CancellationToken ct) => study.GetScenariosAsync(ct);

    [HttpGet("queue")]
    public Task<IReadOnlyList<ScenarioCardDto>> GetQueue(
        [FromQuery] ScenarioCategory? category,
        [FromQuery, Range(0, 100)] int newLimit = 10,
        CancellationToken ct = default) =>
        study.GetStudyQueueAsync(category, newLimit, ct);

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ScenarioCardDto>> Get(Guid id, CancellationToken ct)
    {
        var card = await study.GetScenarioAsync(id, ct);
        return card is null ? NotFound() : card;
    }

    [HttpPost("{id:guid}/steps/{stepId:guid}/answer")]
    public async Task<ActionResult<AnswerResultDto>> SubmitAnswer(
        Guid id, Guid stepId, SubmitAnswerRequest request, CancellationToken ct)
    {
        var (result, error) = await study.SubmitAnswerAsync(id, stepId, request.SelectedOptionId!.Value, ct);

        return error switch
        {
            AnswerError.StepNotFound => NotFound(),
            AnswerError.OptionNotFound => BadRequest("The selected option does not belong to this step."),
            _ => result!,
        };
    }

    [HttpPost("{id:guid}/review")]
    public async Task<ActionResult<UserProgressDto>> SubmitReview(
        Guid id, SubmitReviewRequest request, CancellationToken ct)
    {
        var progress = await study.SubmitReviewAsync(id, request.Rating!.Value, ct);
        return progress is null ? NotFound() : progress;
    }
}
