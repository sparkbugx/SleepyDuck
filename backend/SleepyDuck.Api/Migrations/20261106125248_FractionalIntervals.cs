using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleepyDuck.Api.Migrations
{
    /// <inheritdoc />
    public partial class FractionalIntervals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "IntervalDays",
                table: "UserProgress",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IntervalDays",
                table: "UserProgress",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
