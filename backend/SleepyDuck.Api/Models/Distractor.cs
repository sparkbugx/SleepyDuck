namespace SleepyDuck.Api.Models;

public class Distractor
{
    public Guid Id { get; set; }
    public Guid SequentialStepId { get; set; }
    public required string Text { get; set; }

    /// <summary>Why this plausible-sounding action is wrong.</summary>
    public required string Explanation { get; set; }

    public SequentialStep? SequentialStep { get; set; }
}
