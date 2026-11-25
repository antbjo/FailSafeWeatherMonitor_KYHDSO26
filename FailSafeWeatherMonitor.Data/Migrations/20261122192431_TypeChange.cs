using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FailSafeWeatherMonitor.Migrations;

/// <inheritdoc />
public partial class _20261122192431_TypeChange : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<double>(
            name: "Temperature",
            table: "WeatherDataPoints",
            type: "float",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<double>(
            name: "Humidity",
            table: "WeatherDataPoints",
            type: "float",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddColumn<string>(
            name: "DataSource",
            table: "WeatherDataPoints",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "DataSource",
            table: "WeatherDataPoints");

        migrationBuilder.AlterColumn<int>(
            name: "Temperature",
            table: "WeatherDataPoints",
            type: "int",
            nullable: false,
            oldClrType: typeof(double),
            oldType: "float");

        migrationBuilder.AlterColumn<int>(
            name: "Humidity",
            table: "WeatherDataPoints",
            type: "int",
            nullable: false,
            oldClrType: typeof(double),
            oldType: "float");
    }
}
