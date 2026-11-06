using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleepyDuck.Api.Migrations
{
    /// <summary>
    /// Mastery is stored per card and only recalculated on review, so cards that were
    /// already studied need moving onto the new thresholds here.
    /// </summary>
    public partial class CrammingMasteryThresholds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Recalculate(mastered: 5, reviewing: 2));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Recalculate(mastered: 21, reviewing: 6));
        }

        private static string Recalculate(int mastered, int reviewing) => $"""
            UPDATE "UserProgress"
            SET "MasteryLevel" = CASE
                WHEN "IntervalDays" >= {mastered} THEN 'Mastered'
                WHEN "IntervalDays" >= {reviewing} THEN 'Reviewing'
                ELSE 'Learning'
            END
            WHERE "MasteryLevel" <> 'New';
            """;
    }
}
