using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FailSafeWeatherMonitor.Migrations;

/// <inheritdoc />
public partial class _20261123195031_AddUniqueIndexForWeatherDataPoints : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "StationName",
            table: "WeatherDataPoints",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.CreateIndex(
            name: "IX_WeatherDataPoints_StationName_TimeStamp",
            table: "WeatherDataPoints",
            columns: new[] { "StationName", "TimeStamp" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_WeatherDataPoints_StationName_TimeStamp",
            table: "WeatherDataPoints");

        migrationBuilder.AlterColumn<string>(
            name: "StationName",
            table: "WeatherDataPoints",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");
    }
}
