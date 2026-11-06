using SleepyDuck.Api.Data;
using SleepyDuck.Api.Models;

namespace SleepyDuck.Api.Tests;

public class SeedDataTests
{
    [Fact]
    public void Every_category_has_at_least_two_scenarios()
    {
        foreach (var category in Enum.GetValues<ScenarioCategory>())
        {
            Assert.True(SeedData.Cards.Count(c => c.Category == category) >= 2, category.ToString());
        }
    }

    [Fact]
    public void No_step_repeats_an_option()
    {
        var distractorsByStep = SeedData.Distractors.ToLookup(d => d.SequentialStepId);

        Assert.All(SeedData.Steps, step =>
        {
            var options = distractorsByStep[step.Id].Select(d => d.Text).Append(step.CorrectAction).ToList();

            Assert.Equal(options.Count, options.Distinct().Count());
        });
    }

    public static TheoryData<ScenarioCategory> Categories => new(Enum.GetValues<ScenarioCategory>());

    [Theory]
    [MemberData(nameof(Categories))]
    public void Option_length_is_not_a_reliable_way_to_guess(ScenarioCategory category)
    {
        var cards = SeedData.Cards.Where(c => c.Category == category).Select(c => c.Id).ToHashSet();
        var steps = SeedData.Steps.Where(s => cards.Contains(s.ScenarioCardId)).ToList();
        var distractorsByStep = SeedData.Distractors.ToLookup(d => d.SequentialStepId);

        var correctIsLongest = steps.Count(step =>
            step.CorrectAction.Length > distractorsByStep[step.Id].Max(d => d.Text.Length));
        var correctIsShortest = steps.Count(step =>
            step.CorrectAction.Length < distractorsByStep[step.Id].Min(d => d.Text.Length));

        Assert.True(correctIsLongest * 3 <= steps.Count, $"longest on {correctIsLongest} of {steps.Count} steps");
        Assert.True(correctIsShortest * 3 <= steps.Count, $"shortest on {correctIsShortest} of {steps.Count} steps");
    }

    [Fact]
    public void Every_scenario_is_multi_step()
    {
        var stepCounts = SeedData.Steps.GroupBy(s => s.ScenarioCardId).Select(g => g.Count()).ToList();

        Assert.Equal(SeedData.Cards.Count, stepCounts.Count);
        Assert.All(stepCounts, count => Assert.True(count >= 3));
    }

    [Fact]
    public void Every_step_has_three_distractors()
    {
        var distractorsByStep = SeedData.Distractors.ToLookup(d => d.SequentialStepId);

        Assert.All(SeedData.Steps, step => Assert.Equal(3, distractorsByStep[step.Id].Count()));
    }

    [Fact]
    public void All_ids_are_unique()
    {
        var ids = SeedData.Cards.Select(c => c.Id)
            .Concat(SeedData.Steps.Select(s => s.Id))
            .Concat(SeedData.Steps.Select(s => s.CorrectOptionId))
            .Concat(SeedData.Distractors.Select(d => d.Id))
            .ToList();

        Assert.Equal(ids.Count, ids.Distinct().Count());
    }
}
