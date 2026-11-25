using FailSafeWeatherMonitor.Data;
using FailSafeWeatherMonitor.Models;

namespace FailSafeWeatherMonitor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var _db = new FailSafeWeatherMonitorDbContext();
            var importer = new WeatherImporter(_db);
            importer.ImportFromFile("weather_data.csv");

            await importer.ImportFromApi();
        }
    }
}
