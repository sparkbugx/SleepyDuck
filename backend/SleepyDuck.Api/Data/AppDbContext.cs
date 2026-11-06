using Microsoft.EntityFrameworkCore;
using SleepyDuck.Api.Models;

namespace SleepyDuck.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ScenarioCard> ScenarioCards => Set<ScenarioCard>();
    public DbSet<SequentialStep> SequentialSteps => Set<SequentialStep>();
    public DbSet<Distractor> Distractors => Set<Distractor>();
    public DbSet<UserProgress> UserProgress => Set<UserProgress>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ScenarioCard>(card =>
        {
            card.Property(c => c.Title).HasMaxLength(200);
            card.Property(c => c.Category).HasConversion<string>().HasMaxLength(50);
            card.HasIndex(c => c.Category);

            card.HasMany(c => c.Steps)
                .WithOne(s => s.ScenarioCard)
                .HasForeignKey(s => s.ScenarioCardId)
                .OnDelete(DeleteBehavior.Cascade);

            card.HasOne(c => c.Progress)
                .WithOne(p => p.ScenarioCard)
                .HasForeignKey<UserProgress>(p => p.ScenarioCardId)
                .OnDelete(DeleteBehavior.Cascade);

            card.HasData(SeedData.Cards);
        });

        modelBuilder.Entity<SequentialStep>(step =>
        {
            step.HasIndex(s => new { s.ScenarioCardId, s.Order }).IsUnique();

            step.HasMany(s => s.Distractors)
                .WithOne(d => d.SequentialStep)
                .HasForeignKey(d => d.SequentialStepId)
                .OnDelete(DeleteBehavior.Cascade);

            step.HasData(SeedData.Steps);
        });

        modelBuilder.Entity<Distractor>().HasData(SeedData.Distractors);

        modelBuilder.Entity<UserProgress>(progress =>
        {
            progress.Property(p => p.MasteryLevel).HasConversion<string>().HasMaxLength(20);
            progress.HasIndex(p => p.NextReviewDate);
        });
    }
}
