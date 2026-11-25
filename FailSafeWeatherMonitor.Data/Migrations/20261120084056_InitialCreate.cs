using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FailSafeWeatherMonitor.Migrations;

/// <inheritdoc />
public partial class _20261120084056_InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "WeatherDataPoints",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                Temperature = table.Column<int>(type: "int", nullable: false),
                Humidity = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WeatherDataPoints", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "WeatherDataPoints");
    }
}
