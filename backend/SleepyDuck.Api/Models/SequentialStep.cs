namespace SleepyDuck.Api.Models;

public class SequentialStep
{
    public Guid Id { get; set; }
    public Guid ScenarioCardId { get; set; }

    /// <summary>1-based position of the step within its scenario.</summary>
    public int Order { get; set; }

    public required string Prompt { get; set; }

    /// <summary>
    /// Option id for <see cref="CorrectAction"/>. It sits in the same id space as
    /// <see cref="Distractor.Id"/> so clients can't tell the right answer from the wrong ones.
    /// </summary>
    public Guid CorrectOptionId { get; set; }

    public required string CorrectAction { get; set; }

    /// <summary>Why the correct action is right.</summary>
    public required string Explanation { get; set; }

    public ScenarioCard? ScenarioCard { get; set; }
    public List<Distractor> Distractors { get; set; } = [];
}
