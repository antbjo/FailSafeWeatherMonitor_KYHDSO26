namespace FailSafeWeatherMonitor;

public class WeatherDataPoint
{
    public int Id { get; set; }
    public string StationName { get; set; } = null!;
    public DateTime TimeStamp { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public string DataSource { get; set; } = null!;
}
