namespace rekrutacja.Entities
{
    public class WeatherData
    {
        public double Temp { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public string Description { get; set; }
        public int Humidity { get; set; }

        public string Country { get; set; }
        public string State { get; set; } 
    }
}
