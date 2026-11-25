using FailSafeWeatherMonitor.Data;
using System.Globalization;
using System.Text.Json.Serialization;

namespace FailSafeWeatherMonitor.Models;

public class WeatherImporter(FailSafeWeatherMonitorDbContext _db)
{
    public string BadDataLog = "bad_data.log";

    public void ImportFromFile(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var existingDataPoint = _db.WeatherDataPoints
            .Select(w => new { w.StationName, w.TimeStamp })
            .ToHashSet();

        var seenInFile = new HashSet<string>();
        var validData = new List<WeatherDataPoint>();
        var invalidData = new List<string>();

        for (int i = 1; i < lines.Length; i++)
        {
            int rowNumber = i + 1;
            var parts = lines[i].Split('\t');

            string stationName = parts[0].Trim();

            if (parts.Length < 4)
            {
                invalidData.Add($"Rad {rowNumber}: Not enough columns.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(stationName))
            {
                invalidData.Add($"Row {rowNumber}: Invalid station name.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(parts[1]) 
                || !DateTime.TryParse(parts[1].Trim(), CultureInfo.InvariantCulture, out DateTime timeStamp))
            {
                invalidData.Add($"Row {rowNumber}: Invalid timestamp.");
                continue;
            }

            if (!double.TryParse(parts[2].Trim(), CultureInfo.InvariantCulture, out double temperature) 
                || temperature < -100 
                || temperature > 100)
            {
                invalidData.Add($"Row {rowNumber}: Invalid temperature.");
                continue;
            }

            if (!double.TryParse(parts[3].Trim(), CultureInfo.InvariantCulture, out double humidity) 
                || humidity < 0 
                || humidity > 100)
            {
                invalidData.Add($"Row {rowNumber}: Invalid humidity.");
                continue;
            }

            string unique = $"{stationName}_{timeStamp}";
            if (!seenInFile.Add(unique))
            {
                invalidData.Add($"Row {rowNumber}: Duplicate.");
                continue;
            }

            if (existingDataPoint.Contains(new { StationName = stationName, TimeStamp = timeStamp }))
            {
                continue;
            }

            validData.Add(new WeatherDataPoint
            {
                StationName = stationName,
                TimeStamp = timeStamp,
                Temperature = temperature,
                Humidity = humidity,
                DataSource = "CSV-File"
            });
        }
            _db.WeatherDataPoints.AddRange(validData);
            _db.SaveChanges();

        Console.WriteLine($"Successful import! Valid data points: {validData.Count}. Invalid data points: {invalidData.Count}");

        File.WriteAllLines(BadDataLog, invalidData);
    }

    public async Task ImportFromApi()
    {
        try
        {
            using var http = new HttpClient();
            string url = "https://api.open-meteo.com/v1/forecast?latitude=60.6745&longitude=17.1417&current=temperature_2m,relative_humidity_2m";     // Från Gävle!

            Console.WriteLine("\nImporting data from Open-Meteo, showing the current weather for Gävle, Sweden...");
            var weather = await http.GetFromJsonAsync<WeatherApiDto>(url);
            double temp = weather?.CurrentWeather.Temperature ?? 0;
            double humidity = weather?.CurrentWeather.Humidity ?? 0;

            var dataPointFromApi = new WeatherDataPoint
            {
                StationName = "Gävle",
                TimeStamp = DateTime.UtcNow,
                Temperature = temp,
                Humidity = humidity,
                DataSource = "API"
            };
            _db.WeatherDataPoints.Add(dataPointFromApi);
            await _db.SaveChangesAsync();

            Console.WriteLine($"Current temperature in Gävle: {temp} °C, humidity: {humidity}%");
        }
        catch (HttpRequestException ex)
        {
            Console.Write($"Could not collect data from API: {ex.Message}");
        }

    }

    public record WeatherApiDto(
        [property: JsonPropertyName("current")] CurrentWeatherDto CurrentWeather
    );
    public record CurrentWeatherDto(
        [property: JsonPropertyName("temperature_2m")] double Temperature,
        [property: JsonPropertyName("relative_humidity_2m")] double Humidity

    ); 
}
