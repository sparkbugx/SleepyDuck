using SleepyDuck.Api.Data;

namespace SleepyDuck.Api.Tests;

public class SeedDataTests
{
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
