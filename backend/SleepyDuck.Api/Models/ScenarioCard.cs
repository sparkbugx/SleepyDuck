namespace SleepyDuck.Api.Models;

public class ScenarioCard
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public ScenarioCategory Category { get; set; }
    public required string ProblemStatement { get; set; }
    public required string TechnicalExplanation { get; set; }

    public List<SequentialStep> Steps { get; set; } = [];
    public UserProgress? Progress { get; set; }
}
